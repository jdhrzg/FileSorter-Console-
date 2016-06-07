using System;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Drawing.Imaging;
using System.Text;
using System.Collections.Generic;

namespace FileSorter
{
    class Program
    {
        static void Main(string[] args)
        {
            string dirPath = "";
            bool validPath = false;
            do
            {
                Console.WriteLine(">ENTER THE PATH TO THE FOLDER YOU WANT TO SORT.");
                dirPath = Console.ReadLine();

                validPath = Directory.Exists(dirPath);
            }
            while (!validPath);

            string subdirectories = "";
            bool validSubdirectories = false;
            do
            {
                Console.WriteLine("\n>INCLUDE FILES FROM SUBFOLDERS IN " + dirPath + "?\n>Yes (Y) No (N)");
                subdirectories = Console.ReadLine();
                subdirectories = subdirectories.ToUpper();

                if (subdirectories == "Y" || subdirectories == "N") validSubdirectories = true;
                else validSubdirectories = false;
            }
            while (!validSubdirectories);

            string[] filePaths;
            if (subdirectories == "Y")
            {
                Console.WriteLine("\n>GATHERTING FILES FROM " + dirPath + " (include subfolders)...\n");
                filePaths = Directory.GetFiles(dirPath, "*.*", SearchOption.AllDirectories).ToArray();
            }
            else
            {
                Console.WriteLine("\n>GATHERTING FILES FROM " + dirPath + " only...\n");
                filePaths = Directory.GetFiles(dirPath, "*.*", SearchOption.TopDirectoryOnly).ToArray();
            }
            
            foreach (string filePath in filePaths)
            {
                Console.WriteLine(filePath);
            }
            
            List<FileInfo> FileInfoList = new List<FileInfo>();

            foreach (string filePath in filePaths)
            {
                DateTime date = GetDateTakenOrModified(filePath);
                FileInfoList.Add(new FileInfo(filePath, date));
            }
            Console.WriteLine("\n>FINISHED GATHERTING FILES FROM " + dirPath + ".");

            string groupBy = "";
            bool validSortBy = false;
            do 
            {
                Console.WriteLine("\n>GROUP FILES BY DAY (D), MONTH (M), OR YEAR (Y)?");
                groupBy = Console.ReadLine();
                groupBy = groupBy.ToUpper();

                if (groupBy == "D" || groupBy == "M" || groupBy == "Y") validSortBy = true;
                else validSortBy = false;
            }
            while (!validSortBy);

            string copyOrMove = "";
            bool validCopyOrMove = false;
            do
            {
                Console.WriteLine("\n>MOVE (M) OR COPY (C) THE FILES TO THE NEW FOLDERS?");
                copyOrMove = Console.ReadLine();
                copyOrMove = copyOrMove.ToUpper();

                if (copyOrMove == "C" || copyOrMove == "M") validCopyOrMove = true;
                else validCopyOrMove = false;
            }
            while (!validCopyOrMove);

            // by ascending date
            FileInfoList.Sort(new Comparison<FileInfo>((x, y) => DateTime.Compare(x.Date, y.Date)));

            int copyOrMoveCounter = 0;
            int directoryCounter = 0;
            switch (groupBy)
            {
                case "D":

                    Console.WriteLine("\n>GROUPING FILES BY DAY...");

                    foreach (FileInfo fileInfo in FileInfoList)
                    {
                        string newFileDestination = dirPath + "\\" + fileInfo.Date.Year + "-" + fileInfo.Date.Month + "-" + fileInfo.Date.Day + " " + fileInfo.Date.DayOfWeek;
                        
                        int index = fileInfo.Path.LastIndexOf("\\");
                        string fileName = fileInfo.Path.Substring(index + 1);

                        if (!Directory.Exists(newFileDestination))
                        {
                            Directory.CreateDirectory(newFileDestination);
                            directoryCounter += 1;
                        }
                        
                        if (!File.Exists(newFileDestination + "\\" + fileName))
                        {
                            if (copyOrMove == "C")
                            {
                                File.Copy(fileInfo.Path, (newFileDestination + "\\" + fileName));
                            }
                            else
                            {
                                File.Move(fileInfo.Path, (newFileDestination + "\\" + fileName));
                            }
                            copyOrMoveCounter += 1;
                        }
                    }
                    break;

                case "M":

                    Console.WriteLine("\n>GROUPING FILES BY MONTH...");

                    foreach (FileInfo fileInfo in FileInfoList)
                    {
                        string newFileDestination = dirPath + "\\" + fileInfo.Date.Year + "-" + fileInfo.Date.Month;

                        int index = fileInfo.Path.LastIndexOf("\\");
                        string fileName = fileInfo.Path.Substring(index + 1);

                        if (!Directory.Exists(newFileDestination))
                        {
                            Directory.CreateDirectory(newFileDestination);
                            directoryCounter += 1;
                        }

                        if (!File.Exists(newFileDestination + "\\" + fileName))
                        {
                            if (copyOrMove == "C")
                            {
                                File.Copy(fileInfo.Path, (newFileDestination + "\\" + fileName));
                            }
                            else
                            {
                                File.Move(fileInfo.Path, (newFileDestination + "\\" + fileName));
                            }
                            copyOrMoveCounter += 1;
                        }
                    }
                    break;

                case "Y":

                    Console.WriteLine("\n>GROUPING FILES BY YEAR...");

                    foreach (FileInfo fileInfo in FileInfoList)
                    {
                        string newFileDestination = dirPath + "\\" + fileInfo.Date.Year;

                        int index = fileInfo.Path.LastIndexOf("\\");
                        string fileName = fileInfo.Path.Substring(index + 1);

                        if (!Directory.Exists(newFileDestination))
                        {
                            Directory.CreateDirectory(newFileDestination);
                            directoryCounter += 1;
                        }

                        if (!File.Exists(newFileDestination + "\\" + fileName))
                        {
                            if (copyOrMove == "C")
                            {
                                File.Copy(fileInfo.Path, (newFileDestination + "\\" + fileName));
                            }
                            else
                            {
                                File.Move(fileInfo.Path, (newFileDestination + "\\" + fileName));
                            }
                            copyOrMoveCounter += 1;
                        }
                    }
                    break;
            }

            if (copyOrMove == "C") Console.WriteLine(">PROGRAM FINISHED.\n\n>COPIED " + copyOrMoveCounter + " FILES OF ORIGINAL " + filePaths.Length +
                " FILES IN DIRECTORY.\n>CREATED " + directoryCounter + " NEW FOLDERS.");
            if (copyOrMove == "M") Console.WriteLine(">PROGRAM FINISHED.\n\n>MOVED " + copyOrMoveCounter + " FILES OF ORIGINAL " + filePaths.Length +
                " FILES IN DIRECTORY.\n>CREATED " + directoryCounter + " NEW FOLDERS.");

            Console.WriteLine("\n>PRESS ANY KEY TO EXIT.");
            Console.ReadLine();
        }
        
       /// <summary>
       /// Return the date the image was taken if available or if not the file last modified (FileInfo.LastWriteTime)
       /// </summary>
       /// <param name="filePath"></param>
       /// <returns></returns>
        private static DateTime GetDateTakenOrModified(string filePath)
        {
            string fullDate;
            DateTime date = new System.IO.FileInfo(filePath).LastWriteTime; //assume property is not available from image

            PropertyItem dateProp = null;

            Image myImage=null;

            try
            {
                myImage = Image.FromFile(filePath);
                dateProp = myImage.GetPropertyItem(36867);
            }

            catch (Exception)
            {
                Console.WriteLine(filePath + " (no date taken property - getting date modified).");
            }

            finally
            {
                if (myImage != null)
                {
                    myImage.Dispose();
                    myImage = null;
                }
            }

             if (dateProp != null)
             {
                 // fullDate = "2014:12:29 11:00:05\0"
                 fullDate = Encoding.UTF8.GetString(dateProp.Value).Trim();

                 int seperator = fullDate.IndexOf(" ");
                 // dayMonthYear = "2014:12:29"
                 string dayMonthYear = fullDate.Substring(0, seperator);
                 // dayMonthYear = "2014-12-29"
                 dayMonthYear = dayMonthYear.Replace(":", "-");

                 // time = " 11:00:05\0"
                 string time = fullDate.Substring(seperator, fullDate.Length - seperator);
                 // time = "11:00:05"
                 time = time.Substring(0, 9);

                 // "2014-12-29 11:00:05"
                 date = DateTime.Parse(dayMonthYear + time);
             }


             return date;
        }

        // classes
        public class FileInfo
        {
            //fields
            private string _path;
            private DateTime _date;

            // properties
            public string Path
            {
                get { return this._path; }
                set { this._path = value; }
            }
            public DateTime Date
            {
                get { return this._date; }
                set { this._date = value; }
            }

            // constructor
            public FileInfo(string path, DateTime date)
            {
                this._path = path;
                this._date = date;
            }
        }

    }
}
