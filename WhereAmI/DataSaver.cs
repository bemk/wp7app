using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Runtime.Serialization;
using System.IO.IsolatedStorage;
using System.IO;

namespace WhereAmI
{
    public class DataSaver<WorkoutDatabase>
    {
        private const string TargetFolderName = "WorkoutDatabaseFolder";
        private static DataContractSerializer dataSerializer;
        private static IsolatedStorageFile isoFile2;
        static IsolatedStorageFile isoFile1
        {
            get
            {
                if (isoFile2 == null)
                { isoFile2 = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication(); }
                return isoFile2;
            }
        }

        public DataSaver()
        {
            dataSerializer = new DataContractSerializer(typeof(WorkoutDatabase));
        }

        public static void saveDatabaseToIsolatedStorage(WorkoutDatabase sourceData, String targetFileName)
        {
            string TargetFileName = String.Format("{0}/{1}.dat",TargetFolderName, targetFileName);
            
            if (!isoFile1.DirectoryExists(TargetFolderName))
                isoFile1.CreateDirectory(TargetFolderName);
            try
            {
                using (var targetFile = isoFile1.CreateFile(TargetFileName))
                {
                    dataSerializer.WriteObject(targetFile, sourceData);
                }
            }
            catch (Exception e)
            {
             // isoFile1.DeleteFile(TargetFileName);
                throw e;
            }
            System.Diagnostics.Debug.WriteLine("Available Free Space in Isolated Storage:" + isoFile1.AvailableFreeSpace);
        }

        internal IsolatedStorageFile getIsolatedStorageFile()
        {
            return isoFile1;
        }
        public static WorkoutDatabase loadDatabaseFromIsolatedStorage(string fileName)
        {
            WorkoutDatabase retrievedDatabase = default(WorkoutDatabase);
            string TargetFileName = String.Format("{0}/{1}.dat",TargetFolderName, fileName);
            if (isoFile1.FileExists(TargetFileName) && isoFile1 !=null)
            {
                using (var sourceStream = isoFile1.OpenFile(TargetFileName, FileMode.Open))
                {
                    retrievedDatabase = (WorkoutDatabase)dataSerializer.ReadObject(sourceStream);
                }
            }
            return retrievedDatabase;
        }


    }
}
