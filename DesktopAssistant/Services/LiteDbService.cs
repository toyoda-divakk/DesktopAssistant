using System.Collections;
using System.Text;
using System.Text.Json;
using DesktopAssistant.Contracts.Services;
using DesktopAssistant.Core.Models;
using Humanizer;
using LiteDB;

namespace DesktopAssistant.Services;

// TODO:採番の時に1回Upsertして取り直しをしているが、次に取得するIDが取れるならそうした方が良くない？消したり入れたりしたときにIDの使いまわしが発生したら変な紐づきができて危ないし、その辺の動作確認した方がよさそう。
// TODO:IDの使いまわしが発生しないか確認すること。最後のIDを削除して、次に取得したIDが同じになるか確認する。


/// <summary>
/// LiteDB
/// </summary>
public class LiteDbService(ILocalSettingsService localSettingsService) : ILiteDbService
{
    private readonly string _filename = "data.db";
    private string FilePath => Path.Combine(localSettingsService.ApplicationDataFolder, _filename);
    private LiteDatabase GetContext => new(FilePath);

    public void CreateOrInitializeDatabase()
    {
        var localPath = localSettingsService.ApplicationDataFolder;
        if (IsExistDatabase())
        {
            //データベースファイルが存在している場合は削除
            File.Delete(Path.Combine(localPath, _filename));
        }
    }

    public bool IsExistDatabase() => File.Exists(FilePath);

    public IEnumerable<T> GetTable<T>()
    {
        using var context = GetContext;
        var table = context.GetCollection<T>(typeof(T).Name.Pluralize());   // 複数形に変換してテーブル名にする
        return table.FindAll().ToList();
    }

    public void Upsert<T>(T data)
    {
        using var context = GetContext;
        var table = context.GetCollection<T>(typeof(T).Name.Pluralize());
        table.Upsert(data);
    }

    // 不要。こういうことが必要ないように組む。ListはIgnoreしているので保存されないが、個別に忘れずに保存する。NoSQLがRDBに比べて面倒なのはこういうところだと思った。
    ///// <summary>
    ///// data内にListがある場合、Listの中身を全てUpsertする
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    ///// <param name="data"></param>
    //public void UpsertList<T>(T data)
    //{
    //    // dataの中のListフィールドを全て列挙
    //    var properties = data?.GetType().GetProperties().Where(x => x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(List<>));
    //    foreach (var property in properties!)
    //    {
    //        var list = property.GetValue(data) as IList;
    //        foreach (var item in list!)
    //        {
    //            Upsert(item);
    //        }
    //    }
    //}

    public void Delete<T>(T target) where T : IIdentifiable
    {
        using var context = GetContext;
        var table = context.GetCollection<T>(typeof(T).Name.Pluralize());
        table.Delete(target.Id);
    }

    // IIdentifiableが無ければ-1を返す
    public long GetLastId<T>()
    {
        // TがIIdentifiableを実装しているか
        return typeof(T).GetInterfaces().Contains(typeof(IIdentifiable)) ? GetTable<T>().Max(x => (x as IIdentifiable)!.Id) : -1;
    }

    // TODO:アプリ固有の操作は後でクラスを分離した方が良いかも？
    // TODO:Assistant消す場合も、子のTopicとその中の会話履歴も消さなければならないから、それは専用メソッドにした方が良い。作成したら、Assistantを削除するところ全てに適用すること。
    // TODO:IncludeTopicをオプションにしたいが、その下のMessageまでIncludeするとなるとなかなか重くなるのでは？使わない間はメモリに置かないようにすべきだし。 → どこまで使うかは画面によるので、Listはなるべくnullにしておき、必要になったら取得する考えで。
    public IEnumerable<Assistant> GetAssistants(){
        var assistants = GetTable<Assistant>();
        if (!assistants.Any())
        {
            throw new Exception("Assistantが1件も無いのはおかしい。");
        }
        //// 今まで扱った話題を紐づける
        //if (isIncludeTopic)
        //{
        //    
        //    // TODO:Includeができるか確認すること。
        //    //using var context = GetContext;
        //    //assistants = context.GetCollection<Assistant>().Include(x => x.Topics);

        //    foreach (var assistant in assistants)
        //    {
        //        assistant.Topics = GetTable<Topic>().Where(x => x.AssistantId == assistant.Id).ToList();
        //    }
        //}

        // AssistantsにIsSelectedが1件も無ければ、最初のアシスタントを選択状態にする
        var selectedAssistant = assistants.Where(x => x.IsSelected);

        if (selectedAssistant.Count() > 1)
        {
            // 最初以外のIsSelectedは全てfalseに修正する
            foreach (var assistant in selectedAssistant.Skip(1))
            {
                assistant.IsSelected = false;
                Upsert(assistant);
            }
        }
        else if (!selectedAssistant.Any())
        {
            var assistant = assistants.First();
            assistant.IsSelected = true;
            Upsert(assistant);
        }
        return assistants;
    }
}

