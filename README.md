# Qihoo 360 文件云查杀引擎

## 使用方法:

```C#
public static async Task Main()
{
    string? text = "f351e1fcca0c4ea05fc44d15a17f8b36"; //expmale: WannaCry Malware MD5

    FileHealthResult? result = await FileHealth.CheckAsync(text);

    if (result is not null)
    {
        if (result.IsOperationSuccess)
        {
            Console.WriteLine("Malware Type : {0}", result?.MalwareType);
            Console.WriteLine("File Pop (0-10) : {0}", result?.Pop);
            Console.WriteLine("The length of time the file was found (day) : {0}", result?.Age);
            Console.WriteLine("Probability (0-100) : {0}", result?.Level);
            Console.WriteLine("Detected threats : {0}", result?.IsVerifiedMalware);
            Console.WriteLine("The file has been uploaded : {0}", result?.HasUpload);
        }
        else
        {
            Console.WriteLine("It didn't work out");
        }
    }
    else
    {
        Console.WriteLine("No Result");
    }

    Console.ReadKey();
}
```

```VisualBasic.NET
Public Shared Async Function Main() As Task
    Dim text As String? = "f351e1fcca0c4ea05fc44d15a17f8b36"
    Dim result As FileHealthResult? = Await FileHealth.CheckAsync(text)
    If TypeOf result Is [not] Then Nothing

    If True Then

        If result.IsOperationSuccess Then
            Console.WriteLine("Malware Type : {0}", result?.MalwareType)
            Console.WriteLine("File Pop (0-10) : {0}", result?.Pop)
            Console.WriteLine("The length of time the file was found (day) : {0}", result?.Age)
            Console.WriteLine("Probability (0-100) : {0}", result?.Level)
            Console.WriteLine("Detected threats : {0}", result?.IsVerifiedMalware)
            Console.WriteLine("The file has been uploaded : {0}", result?.HasUpload)
        Else
            Console.WriteLine("It didn't work out")
        End If
    End If

    If True Then
        Console.WriteLine("No Result")
    End If

    Console.ReadKey()
End Function
```

### 示例输出:

```
Malware Type : Win32/Ransom.WannaCry.HxsAEpsA
File Pop (0-10) : 1
The length of time the file was found (day) : 2415
Probability (0-100) : 70
Detected threats : True
The file has been uploaded : False
```

## 安装方法:

1.  直接右键项目-\>管理Nuget程序包-\>搜索’Qihoo.CloudEngine’-\>安装即可

2.  或在PM命令行中输入命令完成安装:

```Package Manager
PM> NuGet\Install-Package Qihoo.CloudEngine
```
