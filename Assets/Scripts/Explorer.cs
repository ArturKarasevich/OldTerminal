using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;


public class Explorer : MonoBehaviour
{
    public class FileNode
    {

        public string name; 
        public NodeType type;
        public List<FileNode> children = new List<FileNode>();
        public FileNode parent;

        public FileNode(string name, NodeType type, FileNode parent = null)
        {
            this.name = name;
            this.type = type;
            this.parent = parent;
        }
    }


    public AppLayer notepadsapplayer;
    public enum NodeType { Drive, Folder, File }
    [Header("Настройки UI")]
    public GameObject filePrefab;   
    public Transform contentParent; 
    public Button backButton;
    public TMP_FontAsset font;

    [Header("Спрайты иконок")]
    public Sprite driveSprite;
    public Sprite folderSprite;
    public Sprite fileSprite;

    private FileNode root;        
    private FileNode currentDir;   


    void Start()
    {
        BuildFileSystem();
        if (backButton != null)
            backButton.onClick.AddListener(GoBack);

        OpenDirectory(root);
    }

    void OpenDirectory(FileNode node)
    {
        currentDir = node;
        RefreshUI();
    }

    void GoBack()
    {
        if (currentDir != null && currentDir.parent != null)
        {
            OpenDirectory(currentDir.parent);
        }
    }

    void RefreshUI()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        if (backButton != null)
            backButton.interactable = currentDir.parent != null;

        foreach (var file in currentDir.children)
        {
            GameObject item = Instantiate(filePrefab, contentParent);

            TMP_Text txt = item.GetComponentInChildren<TMP_Text>();
            txt.text = file.name;
            txt.font = font;

            Image[] images = item.GetComponentsInChildren<Image>();
            foreach (var img in images)
            {
                if (img.gameObject.name == "Icon")
                { 
                    img.sprite = GetSprite(file.type);
                    break;
                }
            }

            Button btn = item.GetComponent<Button>();

            float lastClick = 0;
            btn.onClick.AddListener(() => {
                if (Time.time - lastClick < 0.4f)
                {
                    if (file.type != NodeType.File) OpenDirectory(file);
                    else OpenFile(file);
                }
                lastClick = Time.time;
            });
        }   
    }

    void OpenFile(FileNode file)
    {
        Debug.Log("Открываем файл: " + file.name);
        if (notepadsapplayer == null) return;
        Notepad notepad = notepadsapplayer.transform.GetComponentInChildren(typeof(Notepad)) as Notepad;
        if (notepad != null)
        {
            notepad.OpenFile(file.name, GetFileData(file.name));
        }
    }

    string GetFileData(string fileName)
    {
        switch (fileName)
        {
            // Папка "Мысли заметки идеи"

            case "room_sania.txt":
                return "Заходил к Сане. \nКомната почти такая же по размеру. \nУ меня все как склад. \nУ него будто там жизнь. \nСказал что ему \"в самый раз\". \n…\nКот опять лег за ноутом.";

            case "system_garbage.txt":
                return "Макс опять залил \"идеальный\" софт.\nВ итоге: битая сборка, мусор в логах, два часа на поиск, еще полдня на чистку.\nОн еще говорит что локально у него все работало.\nДмитрий пишет про сроки. Очень вовремя.";

            case "Access_logic.bat":
                return "Executing logic check...\n" +
                       "Error: Field 'Moral_Compass' not found in user 'Max'.\n" +
                       "Redirecting energy to 'Atlas' project...\n" +
                       "Status: Overloaded.";

            case "uvolnenie.txt":
                return "Генеральному директору ООО NewComputerEra\r\nА.Ф.Галимову\r\nОт backend разработчика Е.А.Бурлакова\r\nЗАЯВЛЕНИЕ\r\nВ соответствии со статьей 80 Трудового кодекса Российской Федерации прошу уволить меня по собственному желанию\r\n\r\n15/03/25 ______/Е.А.Бурлаков";

            case "na_krishe_doma_tvoego.txt":
                return "\"Вспомнил, как мы после универа ночью сидели на крыше с саней, после универа. Холодно было так что пальцы не слушались, а он все косился на люк и боялся,  что его ветром захлопнет и нас оставят там до утра. Я тогда сказал, что хочу однажды сорваться и уехать в горы. Просто взять и  исчезнуть. Без звонков, дедлайнов, бесконечных \"надо. Он посмотрел на меня так будто  я не в горы собрался, а в космос без скафандра. -Егор а меня и дома все устраивает. Кот. Свой угол. Ноут.\r\nИ я ведь правда за него рад Честно.\r\nЕсть люди, которые умеют быть счастливыми в коробке. Они обживают ее, ставят кружку\r\nна привычное место, заводят кота, покупают лампу потеплее - и им хорошо.\r\nНо я так не могу. Мне в какой-то момент даже тишина начинает напоминать клетку.";

            case "atlas_present.txt":
                return "Завтра эта гребанная презентация. Дмитрий весь день ходит за мной и зудит.\r\n-Егор, надень рубашку.\r\n-Егор, включи камеру.\r\n-Егор, улыбайся!\r\n-Они хотят видеть лидера разработки.\r\nЗачем? Если они хотят видеть лидера разработки, то пусть смотрят на код и\r\nархитектуру. Но нет. Им нужно лицо. Им мало того что система работает, им надо чтобы она\r\nеще и улыбалась. А я снова должен делать вид что мне это нравится. Я программист.\r\nНе шоумен.";
            case "pismo_v_korzine.txt":
                return "Письмо об увольнении до сих пор лежит в корзине. Даже не удалил.\r\nВесь день чинил чужие ошибки, слушал чужую уверенность, кивал на чужие идеи,\r\nа потом снова открыл корзину и посмотрел на файл.\r\nБудто это не документ, а аварийный выход. Самое мерзкое, что я уже не понимаю\r\nя хочу уйти потому что мне плохо, или потому что слишком долго надеялся что станет лучше.";
            case "zloy1.txt":
                return "Иногда мне кажется, что  мир специально собирается в одну точку, чтобы проверить через  сколько \r\nминут я сорвусь. С утра сервер падает, в обед макс присылает \"маленькую\" правку,\r\nпосле которой летит все. К вечеру созвон где три человека обсуждают 40 минут\r\nкак назвать кнопку. А потом говорят\r\n-Егор, ты какой-то напряженный в последнее время.\r\nДа неужели. Интересно почему?";
            case "zloy2.txt":
                return "Я не злой. Это почему-то все решили если я молчу то мне нормально. Нет.\r\nДаже если я все скажу-меня не поймут. Они не команда. Они просто группа людей,\r\nкоторая привыкла что самый упрямый дотянет до конца. А потом удивляются почему у него такое\r\nвыражение лица.";
            case "nochnoy_commit.txt":
                return "Самые честные мысли почему-то приходят после полуночи. Когда офис пустой. Когда\r\nв окне отражается только монитор и твое уставшее лицо, как у человека который\r\nслишком долго не уходил домой.\r\nЯ сижу, смотрю на строчки кода и думаю именно так и ломаются люди, не громко, не красиво\r\nне в один день, а по чуть-чуть.\r\nКоммит за коммитом. Правка за правкой. Созвон за созвоном. А потом смотришь на часы - 02:27\r\nИ понимаешь что снова отдал куску железа и чужим хотелкам кусок себя.";
            case "ot_sebia.txt":
                return "Я всегда думал, что однажды станет легче. Вот дожму релиз - станет легче. Выбью отгул - станет легче. Закрою квартал - станет легче. Вот уйду - станет легче.\r\nА потом понял одну неприятную вещь. Если внутри тебя что-то треснуло место не всегда виновато.\r\nИногда ты уносишь эту усталость с собой. Из офиса - домой. Из дома - в метро. Из метро - в сон.\r\nИз сна - в утро. И все что у тебя по-настоящему осталось это либо наконец признать что так больше нельзя\r\nили верить в то, что еще немного и все наладится.\r\n";
            case "na_kurilke.txt":
                return "Я не курю, но иногда все равно выхожу к курилке. Потому что, это пожалуй самое\r\nчестное место в офисе. На совещаниях все бодрые, собранные, правильные. Обсуждают планы,\r\nрост, цели. А здесь... Здесь никто ничего не доказывает. Здесь дизайнер устал от бесконечных правок.\r\nТестировщик уже заранее не верит в новый билд. Кто-то говорит \"да все нормально\", и \r\nсразу понятно, что совсем не нормально. Это странное место. Здесь почти никто не улыбается,\r\nзато почти никто и не притворяется.\r\nНаверное поэтому мне здесь легче. Потому что это единственное место в офисе, где перестают играть.";

            case "Alps_trip.txt":
                return "Если в этот раз реально выплатят, должно хватить.\r\nБез шика но хватить.\r\nТретий год одно и тоже:\r\n-Проект горит\r\n-Вот только не сейчас\r\n-Срочные правки\r\nИ потом ты никуда не едешь.\r\nНадо хотя бы раз уехать нормально.\r\nБез ноутбука.";

             // Файлы из папки "Битые файлы"

            case "12.07.2019.png":
            case "C++ libraly.pdf":
            case "Pitolin_posobie.pdf":
            case "chess.exe":
            case "cool_phrases2015.txt":
            case "dump_stack_trace.tmp":
            case "konchitsa_leto.mp3":
            case "my_school_reward.jpg":
            case "sector_0x88.bin":
            case "shturmuia_nebesa.mp3":
            case "temp_cache_atlas.tmp":
            case "unknown_format_01":
            case "Witcher_wallpapper.png":
            case "The_Warriors_of_the_World.mp3":
            case "error_log_772.db":
            case "What_to_take_to_mountains.docx":
                return "not_founded";

            // По умолчанию
            default:
                return "Файл поврежден или имеет неизвестный формат.";
        }
    }

    Sprite GetSprite(NodeType type)
    {
        return type switch
        {
            NodeType.Drive => driveSprite,
            NodeType.Folder => folderSprite,
            _ => fileSprite
        };
    }

    void BuildFileSystem()
    {
        root = new FileNode("Этот компьютер", NodeType.Folder);

        FileNode rootC = new FileNode("C:", NodeType.Drive, root);
        root.children.Add(rootC);
        FileNode rootE = new FileNode("E:", NodeType.Drive, root); 
        root.children.Add(rootE);

        //C

        FileNode users = new FileNode("Users", NodeType.Folder, rootC);
        rootC.children.Add(users);
        FileNode egor = new FileNode("Egor", NodeType.Folder, rootC);
        users.children.Add(egor);
        FileNode desktop = new FileNode("Desktop", NodeType.Folder, egor); 
        egor.children.Add(desktop);
        desktop.children.Add(new FileNode("cmd.bat", NodeType.File, desktop));
        desktop.children.Add(new FileNode("nochnoy_commit.txt", NodeType.File, desktop));
        desktop.children.Add(new FileNode("README_ME_FIRST.txt", NodeType.File, desktop));

        FileNode documents = new FileNode("Documents", NodeType.Folder, egor);
        egor.children.Add(documents);
        FileNode notesC = new FileNode("Notes", NodeType.Folder, documents);
        documents.children.Add(notesC);
        notesC.children.Add(new FileNode("na_krishe_doma_tvoego.txt", NodeType.File, notesC));
        notesC.children.Add(new FileNode("atlas_present.txt", NodeType.File, notesC));
        notesC.children.Add(new FileNode("pismo_v_korzine.txt", NodeType.File, notesC));
        notesC.children.Add(new FileNode("zloy1.txt", NodeType.File, notesC));
        notesC.children.Add(new FileNode("zloy2.txt", NodeType.File, notesC));
        FileNode work = new FileNode("Work", NodeType.Folder, documents);
        documents.children.Add(work);
        work.children.Add(new FileNode("game1.exe", NodeType.File, work));

        FileNode downloads = new FileNode("Downloads", NodeType.Folder, egor);
        egor.children.Add(downloads);
        downloads.children.Add(new FileNode("shamoni_prices.html", NodeType.File, downloads));
        downloads.children.Add(new FileNode("montblanc_routes.pdf", NodeType.File, downloads));

        FileNode pictures = new FileNode("Pictures", NodeType.Folder, egor);
        egor.children.Add(pictures);
        pictures.children.Add(new FileNode("my_school_reward.jpg", NodeType.File, downloads));
        pictures.children.Add(new FileNode("game2.exe", NodeType.File, downloads));

        FileNode trashbox = new FileNode("Корзина", NodeType.Folder, egor);
        trashbox.children.Add(new FileNode("uvolnenie.txt", NodeType.File, trashbox));
        egor.children.Add(trashbox);
        FileNode appdata = new FileNode("AppData", NodeType.Folder, egor);
        egor.children.Add(appdata);
        FileNode local = new FileNode("Local", NodeType.Folder, appdata);
        appdata.children.Add(local);
        FileNode terminalcache = new FileNode("TerminalCache", NodeType.Folder, local);
        local.children.Add(terminalcache);
        terminalcache.children.Add(new FileNode("cmd_history.log", NodeType.File, terminalcache));
        terminalcache.children.Add(new FileNode("game3.exe", NodeType.File, terminalcache));
        FileNode roaming = new FileNode("Roaming", NodeType.Folder, appdata);
        appdata.children.Add(roaming);
        FileNode atlasshell = new FileNode("AtlasShell", NodeType.Folder, roaming);
        roaming.children.Add(atlasshell);
        atlasshell.children.Add(new FileNode("last_login.cfg", NodeType.File, atlasshell));


        rootC.children.Add(new FileNode("ot_sebia.txt", NodeType.File, rootC));
        rootC.children.Add(new FileNode("na_kurilke.txt", NodeType.File, rootC));


        // E
        // Файлы в корне
        rootE.children.Add(new FileNode("Access_logic.bat", NodeType.File, rootE));

        // Пустые папки
        rootE.children.Add(new FileNode("Games", NodeType.Folder, rootE));
        rootE.children.Add(new FileNode("Музыка на задний", NodeType.Folder, rootE));

        // Папка "Мысли заметки идеи"
        FileNode notes = new FileNode("Мысли заметки идеи", NodeType.Folder, rootE);
        notes.children.Add(new FileNode("Alps_trip.txt", NodeType.File, notes));
        notes.children.Add(new FileNode("room_sania.txt", NodeType.File, notes));
        notes.children.Add(new FileNode("system_garbage.txt", NodeType.File, notes));
        rootE.children.Add(notes);

        // Папка "Битые файлы"
        FileNode broken = new FileNode("Битые файлы", NodeType.Folder, rootE);
        string[] bFiles = {
            "12.07.2019.png", "C++ libraly.pdf", "chess.exe", "cool_phrases2015.txt",
            "dump_stack_trace.tmp", "konchitsa_leto.mp3", "my_school_reward.jpg",
            "Pitolin_posobie.pdf", "RECOVERY_TOOL.bat", "sector_0x88.bin",
            "shturmuia_nebesa.mp3", "temp_cache_atlas.tmp", "The_Warriors_of_the_World.mp3",
            "unknown_format_01", "Witcher_wallpapper.png", "Что взять с собой в горы?.docx"
        };

        // Добавляем и сортируем по алфавиту
        foreach (var name in bFiles.OrderBy(x => x))
            broken.children.Add(new FileNode(name, NodeType.File, broken));

        rootE.children.Add(broken);
    }
}