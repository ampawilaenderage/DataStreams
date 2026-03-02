using System;
using System.IO;

class Datastream
{
    static void Main()
    {
        try
        {
            Console.WriteLine("Starting program...\n");

            // Build base path once
            string basisPfad = Path.GetFullPath(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Ordner"));

            // Call methods
            ExecuteFileInfoTasks(basisPfad);
            ExecuteDirectoryTasks(basisPfad);
            ExecuteDirectoryInfoTasks(basisPfad);

            ExecutePathTasks();
            ExecuteDriveInfoTasks();
            ExecuteEnvironmentTasks();
            ExecuteFileStreamTasks(basisPfad);

            ExecuteAdvancedFileStreamTasks(basisPfad);

            ExecuteTextWriterReaderTasks();

            Console.WriteLine("\nFinished.");

            Console.WriteLine("\nFinished.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Unexpected error: " + ex.Message);
        }
    }


    static void ExecuteFileInfoTasks(string basisPfad)
    {
        Console.WriteLine("---- FILEINFO TASKS ----");

        string unterordnerPfad = Path.Combine(basisPfad, "Unterordner");
        string demoPfad = Path.Combine(basisPfad, "Demo.txt");

        FileInfo fi = new FileInfo(demoPfad);

        if (!fi.Exists)
        {
            Console.WriteLine("Demo.txt does not exist.");
            return;
        }

        // Copy as fiKopie.txt (overwrite allowed)
        string fiKopiePfad = Path.Combine(unterordnerPfad, "fiKopie.txt");
        fi.CopyTo(fiKopiePfad, true);
        Console.WriteLine("Copied as fiKopie.txt");

        // Delete fiKopie.txt
        FileInfo fiKopie = new FileInfo(fiKopiePfad);
        if (fiKopie.Exists)
        {
            fiKopie.Delete();
            Console.WriteLine("fiKopie.txt deleted.");
        }

        // Move Demo.txt as Kopie.txt
        string kopiePfad = Path.Combine(unterordnerPfad, "Kopie.txt");

        if (File.Exists(kopiePfad))
        {
            Console.WriteLine("Kopie.txt already exists.");
        }
        else
        {
            fi.MoveTo(kopiePfad);
            Console.WriteLine("Demo.txt moved as Kopie.txt.");
        }

        // Creation time
        if (fi.Exists)
        {
            Console.WriteLine("Created on: " + fi.CreationTime);
        }

        Console.WriteLine();
    }


    static void ExecuteDirectoryTasks(string basisPfad)
    {
        Console.WriteLine("---- DIRECTORY TASKS ----");

        // EXE Path
        string exePfad = AppDomain.CurrentDomain.BaseDirectory;
        Console.WriteLine("EXE Path: " + exePfad);

        // Files in EXE directory
        Console.WriteLine("\nFiles in EXE directory:");
        string[] exeFiles = Directory.GetFiles(exePfad);

        foreach (string file in exeFiles)
        {
            Console.WriteLine(Path.GetFileName(file));
        }

        // All .txt files in Ordner and subdirectories
        Console.WriteLine("\nAll .txt files in 'Ordner':");

        string[] txtFiles = Directory.GetFiles(
            basisPfad,
            "*.txt",
            SearchOption.AllDirectories);

        foreach (string file in txtFiles)
        {
            Console.WriteLine(file);
        }

        Console.WriteLine();
    }


    static void ExecuteDirectoryInfoTasks(string basisPfad)
    {
        Console.WriteLine("---- DIRECTORYINFO TASKS ----");

        string unterordnerPfad = Path.Combine(basisPfad, "Unterordner");

        DirectoryInfo di = new DirectoryInfo(unterordnerPfad);

        if (!di.Exists)
        {
            Console.WriteLine("Unterordner does not exist.");
            return;
        }

        Console.WriteLine("Parent directory: " + di.Parent.Name);
        Console.WriteLine("Root directory: " + di.Root.Name);

        Console.WriteLine();
    }

    static void ExecutePathTasks()
    {
        Console.WriteLine("---- PATH TASKS ----");

        string dummyFile = "X:/Ordner/Unterordner/Datei.ext";

        Console.WriteLine("Directory: " + Path.GetDirectoryName(dummyFile));
        Console.WriteLine("Extension: " + Path.GetExtension(dummyFile));
        Console.WriteLine("File name (with ext): " + Path.GetFileName(dummyFile));
        Console.WriteLine("File name (without ext): " + Path.GetFileNameWithoutExtension(dummyFile));

        // Create temp file
        string tempFile = Path.GetTempFileName();
        FileInfo fi = new FileInfo(tempFile);

        Console.WriteLine("\nTemp file created:");
        Console.WriteLine("Full path: " + fi.FullName);
        Console.WriteLine("Size: " + fi.Length + " bytes");

        fi.Delete();
        Console.WriteLine("Temp file deleted.\n");
    }

    static void ExecuteDriveInfoTasks()
    {
        Console.WriteLine("---- DRIVEINFO TASKS ----");

        DriveInfo[] drives = DriveInfo.GetDrives();

        foreach (DriveInfo drive in drives)
        {
            Console.WriteLine("\nDrive: " + drive.Name);
            Console.WriteLine("Type: " + drive.DriveType);

            if (drive.IsReady)
            {
                Console.WriteLine("Volume label: " + drive.VolumeLabel);
                Console.WriteLine("File system: " + drive.DriveFormat);
                Console.WriteLine("Total size: " + drive.TotalSize);
                Console.WriteLine("Total free space: " + drive.TotalFreeSpace);
                Console.WriteLine("Available free space: " + drive.AvailableFreeSpace);
            }
            else
            {
                Console.WriteLine("Drive not ready.");
            }
        }

        Console.WriteLine();
    }

    static void ExecuteEnvironmentTasks()
    {
        Console.WriteLine("---- ENVIRONMENT SPECIAL FOLDERS ----");

        foreach (Environment.SpecialFolder folder in
                 Enum.GetValues(typeof(Environment.SpecialFolder)))
        {
            string path = Environment.GetFolderPath(folder);

            if (!string.IsNullOrEmpty(path))
            {
                Console.WriteLine(folder + " → " + path);
            }
        }

        Console.WriteLine();
    }

    static void ExecuteFileStreamTasks(string basisPfad)
    {
        Console.WriteLine("---- FILESTREAM TASKS ----");

        string demoPfad = Path.Combine(basisPfad, "Demo.txt");

        if (!File.Exists(demoPfad))
        {
            Console.WriteLine("Demo.txt not found.");
            return;
        }

        using (FileStream fs = new FileStream(
            demoPfad,
            FileMode.Open,
            FileAccess.ReadWrite))
        {
            byte[] daten = new byte[40];

            // Read 20 bytes into array starting at index 10
            int bytesRead = fs.Read(daten, 10, 20);

            Console.WriteLine("Bytes actually read: " + bytesRead);

            Console.WriteLine("Array content as char:");
            foreach (byte b in daten)
            {
                Console.Write((char)b);
            }

            Console.WriteLine();

            // Modify positions 4–6
            daten[4] = (byte)'X';
            daten[5] = (byte)'Y';
            daten[6] = (byte)'Z';

            // Write only these 3 bytes back
            fs.Write(daten, 4, 3);
        }

        Console.WriteLine("\nFileStream closed.\n");
    }

    static void ExecuteAdvancedFileStreamTasks(string basisPfad)
    {
        Console.WriteLine("---- ADVANCED FILESTREAM ----");

        string demoPfad = Path.Combine(basisPfad, "Demo.txt");

        if (!File.Exists(demoPfad))
        {
            Console.WriteLine("Demo.txt not found.");
            return;
        }

        using (FileStream fs = new FileStream(
            demoPfad,
            FileMode.Open,
            FileAccess.ReadWrite))
        {
            byte[] buffer = new byte[] { (byte)'A', (byte)'B', (byte)'C' };

            // 1️⃣ 8 bytes BEFORE end of file
            fs.Seek(-8, SeekOrigin.End);
            fs.Write(buffer, 0, 3);
            Console.WriteLine("Written 8 bytes before end.");

            // 2️⃣ 10 bytes AFTER current position
            fs.Seek(10, SeekOrigin.Current);
            fs.Write(buffer, 0, 3);
            Console.WriteLine("Written 10 bytes after current position.");

            // 3️⃣ ABSOLUTE position at 2nd byte (index 1)
            fs.Seek(1, SeekOrigin.Begin);
            fs.Write(buffer, 0, 3);
            Console.WriteLine("Written at absolute position 2.");
        }

        Console.WriteLine();
    }

    static void DatenSchreiben(string text)
    {
        using (StreamWriter sw = new StreamWriter("dummy.txt", false))
        {
            sw.WriteLine(DateTime.Now + " - " + text);
        }
    }

    static void DatenLesen()
    {
        if (!File.Exists("dummy.txt"))
        {
            Console.WriteLine("dummy.txt not found.");
            return;
        }

        using (StreamReader sr = new StreamReader("dummy.txt"))
        {
            string content = sr.ReadToEnd();
            Console.WriteLine("File content:");
            Console.WriteLine(content);
        }
    }

    static void DatenAnfügen(string text)
    {
        using (StreamWriter sw = new StreamWriter("dummy.txt", true))
        {
            sw.WriteLine(DateTime.Now + " - " + text);
        }
    }

    static void ExecuteTextWriterReaderTasks()
    {
        Console.WriteLine("---- TEXTWRITER / TEXTREADER ----");

        // Write values
        DatenSchreiben("Ich bin ein Test");
        DatenAnfügen(3.141.ToString());
        DatenAnfügen(true.ToString());

        DatenLesen();

        // Append new text
        DatenAnfügen("Ich wurde angefügt");

        Console.WriteLine("\nAfter appending:");
        DatenLesen();

        Console.WriteLine();
    }
}