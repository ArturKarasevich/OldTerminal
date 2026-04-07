using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using static TreeEditor.TreeEditorHelper;

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

    [Header("Спрайты иконок")]
    public Sprite driveSprite;
    public Sprite folderSprite;
    public Sprite fileSprite;

    private FileNode root;        
    private FileNode currentDir;   

    void Awake()
    {
        BuildFileSystem();
    }

    void Start()
    {
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

            Image[] images = item.GetComponentsInChildren<Image>();
            foreach (var img in images)
            {
                if (img.gameObject.name == "Icon")
                { 
                    img.sprite = GetSprite(file.type);
                    break;
                }
            }

            // Вешаем логику клика
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

            case "Alps_trip.txt":
                return "Посмотрел туры в Альпы на декабрь. Шамони, Монблан… Ценник конский, но если закрою бонус за Атлас должно хватить.\n\n" +
                       "Upd: Дмитрий Анатольевич сегодня зашел, похлопал по плечу: \"Егор, проект горит, какой отпуск?\". " +
                       "Это уже третий раз. Третий год я никуда не лечу из-за косяков Макса и \"срочных патчей\". " +
                       "Альпы я увижу только в браузере. Кажется, я так и застряну в этом офисном кресле, так и не дойдя до вершины. Слов нет.";

            case "room_sania.txt":
                return "Зашел к Сане. Планировка один в один как у меня. Те же 4 стены. Но как-то у него живее что ли…\n\n" +
                       "Стою в дверях и чувствую, что стены будто давят. Говорю: \"Сань, тут же тесно\", а он взгляд на меня поднял и ответил: \"Мне в самый раз\".\n\n" +
                       "Upd: А еще у него кот всегда за ноутом лежит.";

            case "system_garbage.txt":
                return "Макс – сверхразум. Опять залил битый мерж, все упало. Весь день чистил за ним логи. " +
                       "Олег орет, что сроки просраны, а Макс в это время заказывает пиццу и рассуждает о «свободном искусстве кода». " +
                       "Иногда мне кажется, что я работаю не с программистом, а с генератором случайных проблем.";

            case "Access_logic.bat":
                return "Executing logic check...\n" +
                       "Error: Field 'Moral_Compass' not found in user 'Max'.\n" +
                       "Redirecting energy to 'Atlas' project...\n" +
                       "Status: Overloaded.";

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

        FileNode rootE = new FileNode("E:", NodeType.Drive, root); 
        root.children.Add(rootE);

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