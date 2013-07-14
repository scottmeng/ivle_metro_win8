using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace icreate_test2.Utils
{
    static class DataManager
    {
        private static List<DataStructure.Module> _modules;
        private static List<DataStructure.Announcement> _announcements;
        private static List<DataStructure.Class> _allClasses;
        private static List<DataStructure.Class>[] _classesForEachDay;
        private static List<DataStructure.SemesterInfo> _sems;
        public static List<DataStructure.SearchResult> searchResults;

        public static void InitializeDataLists()
        {
            _modules = new List<DataStructure.Module>();
            _announcements = new List<DataStructure.Announcement>();
            _allClasses = new List<DataStructure.Class>();
            _sems = new List<DataStructure.SemesterInfo>();

            _classesForEachDay = new List<DataStructure.Class>[6] { new List<DataStructure.Class>(), new List<DataStructure.Class>(), 
                                                                    new List<DataStructure.Class>(), new List<DataStructure.Class>(), 
                                                                    new List<DataStructure.Class>(), new List<DataStructure.Class>()};

            searchResults = new List<DataStructure.SearchResult>();
        }

        public static List<DataStructure.Module> GetModules()
        {
            return _modules;
        }

        public static List<DataStructure.Announcement> GetAnnouncements()
        {
            return _announcements;
        }

        public static List<DataStructure.Class> GetClasses()
        {
            return _allClasses;
        }

        public static List<DataStructure.SemesterInfo> GetSems()
        {
            return _sems;
        }

        public static void AddModule(DataStructure.Module module)
        {
            _modules.Add(module);
        }

        public static void AddAnnouncement(DataStructure.Announcement announcement)
        {
            _announcements.Add(announcement);
        }

        public static void AddClass(DataStructure.Class newClass)
        {
            _allClasses.Add(newClass);
        }

        public static void AddSemInfo(DataStructure.SemesterInfo sem)
        {
            if(!_sems.Contains(sem, new SeminfoEqualityComparer()))
            {
                _sems.Add(sem);
            }
        }

        public static void SortAnnouncementWrtTime()
        {
            _announcements.Sort(new AnnouncementTimeComparer());
        }

        public static int GetModuleIndex(DataStructure.Module module)
        {
            return _modules.IndexOf(module);
        }
        
        public static int GetModuleIndexByModuleId(string moduleId)
        {
            int index = 0;
            while (_modules[index].moduleId != moduleId)
            {
                index++;
            }

            return index;
        }

        public static int GetAnnouncementIndex(int moduleIndex, String announcementId)
        {
            int index = 0;
            while (_modules[moduleIndex].moduleAnnouncements[index].announceID != announcementId)
            {
                index++;
            }
            return index;
        }

        public static DataStructure.Module GetModuleAt(int index)
        {
            return _modules[index];
        }

        public static Color GetModuleColorByCode(string moduleCode)
        {
            Color moduleColor = new Color();
            
            foreach (DataStructure.Module module in _modules)
            {
                if (module.moduleCode.Equals(moduleCode))
                {
                    moduleColor = module.moduleColor;
                }
            }

            return moduleColor;
        }

        public static List<DataStructure.Class> GetDailyClassList(int dayCode)
        {
            return _classesForEachDay[dayCode];
        }

        public static void GenerateDailyClassList()
        {
            // catogorize all classes according to week day 
            foreach(DataStructure.Class mClass in _allClasses)
            {
                switch (mClass.classDayCodeInt)
                {
                    case 1:
                        _classesForEachDay[0].Add(mClass);
                        break;
                    case 2:
                        _classesForEachDay[1].Add(mClass);
                        break;
                    case 3:
                        _classesForEachDay[2].Add(mClass);
                        break;
                    case 4:
                        _classesForEachDay[3].Add(mClass);
                        break;
                    case 5:
                        _classesForEachDay[4].Add(mClass);
                        break;
                    case 6:
                        _classesForEachDay[5].Add(mClass);
                        break;
                    default:
                        break;
                }
            }

            foreach (List<DataStructure.Class> classes in _classesForEachDay)
            {
                classes.Sort(new ClassTimeComparer());
            }
        }


        // generate search results from available modules and announcements
        public static void GenerateSearchResults()
        {
            foreach (DataStructure.Module module in Utils.DataManager.GetModules())
            {
                searchResults.Add(new DataStructure.SearchResult(module.moduleCode, module.moduleName));

                foreach (DataStructure.Announcement announcement in module.moduleAnnouncements)
                {
                    searchResults.Add(new DataStructure.SearchResult(announcement.announceName, announcement.announceContent));
                }
            }
        }
    }


    class SeminfoEqualityComparer : IEqualityComparer<DataStructure.SemesterInfo>
    {
        public bool Equals(DataStructure.SemesterInfo s1, DataStructure.SemesterInfo s2)
        {
            if (s1.Semester == s2.Semester && s1.AcademicYear == s2.AcademicYear)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(DataStructure.SemesterInfo sem)
        {
            int hCode = sem.AcademicYear.Length ^ sem.Semester.Length;
            return hCode.GetHashCode();
        }
    }

    class AnnouncementTimeComparer : IComparer<DataStructure.Announcement>
    {
        // Compares the published date of the announcement
        public int Compare(DataStructure.Announcement announce1, DataStructure.Announcement announce2)
        {
            if (announce1.announceTime.CompareTo(announce2.announceTime) != 0)
            {
                return announce1.announceTime.CompareTo(announce2.announceTime);
            }
            else if (announce1.announceIsRead.CompareTo(announce2.announceIsRead) != 0)
            {
                return announce1.announceIsRead.CompareTo(announce2.announceIsRead);
            }
            else
            {
                return announce1.announceName.CompareTo(announce2.announceName);
            }
        }
    }

    class ClassTimeComparer : IComparer<DataStructure.Class>
    {
        // Compares the starting time of classes
        public int Compare(DataStructure.Class class1, DataStructure.Class class2)
        {
            return class1.classTimePoint.CompareTo(class2.classTimePoint);
        }
    }

}
