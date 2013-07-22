using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using System.Collections.ObjectModel;

namespace icreate_test2.Utils
{
    static class DataManager
    {
        public static ObservableCollection<DataStructure.Module> modules { get; set; }
        public static ObservableCollection<DataStructure.Announcement> announcements { get; set; }

        private static List<DataStructure.Announcement> _announcements;
        private static List<DataStructure.Class> _allClasses;
        private static List<DataStructure.Class>[] _classesForEachDay;
        private static List<DataStructure.SemesterInfo> _sems;
        public static List<DataStructure.Searchable> searchables;

        public static void InitializeDataLists()
        {
            modules = new ObservableCollection<DataStructure.Module>();
            announcements = new ObservableCollection<DataStructure.Announcement>();

            _announcements = new List<DataStructure.Announcement>();
            _allClasses = new List<DataStructure.Class>();
            _sems = new List<DataStructure.SemesterInfo>();

            _classesForEachDay = new List<DataStructure.Class>[6] { new List<DataStructure.Class>(), new List<DataStructure.Class>(), 
                                                                    new List<DataStructure.Class>(), new List<DataStructure.Class>(), 
                                                                    new List<DataStructure.Class>(), new List<DataStructure.Class>()};

            searchables = new List<DataStructure.Searchable>();
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
            modules.Add(module);
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

        public static void ClearAnnouncement()
        {
            _announcements.Clear();
        }

        public static void UpdateModules(DataStructure.Module[] newModules)
        {
            foreach (DataStructure.Module module in modules)
            {
                // if old module no longer exists
                // remove from observable collection
                if (!newModules.Contains(module, new ModuleEqualityComparer()))
                {
                    modules.Remove(module);
                }
                else
                {
                    // if old module still exists
                    // update its announcements
                    foreach (DataStructure.Module newModule in newModules)
                    {
                        if (newModule.moduleId == module.moduleId)
                        {
                            module.moduleAnnouncements = newModule.moduleAnnouncements;
                            break;
                        }
                    }
                }
            }

            // if there are new modules
            foreach (DataStructure.Module newModule in newModules)
            {
                if (!modules.Contains(newModule, new ModuleEqualityComparer()))
                {
                    modules.Add(newModule);
                }
            }

            SortAnnouncementWrtTime();
        }

        public static void SortAnnouncementWrtTime()
        {
            int iterator = 0;
            // sort the list
            _announcements.Sort(new AnnouncementTimeComparer());

            if (announcements.Count == 0)
            {
                foreach (DataStructure.Announcement announcement in _announcements)
                {
                    announcements.Add(announcement);
                }
            }
            else
            {
                foreach (DataStructure.Announcement announcement in announcements)
                {
                    if (!announcements.Contains(announcement, new AnnouncementEqualityComparer()))
                    {
                        announcements.Remove(announcement);
                    }
                }

                foreach (DataStructure.Announcement newAnnouncement in _announcements)
                {
                    if (!announcements.Contains(newAnnouncement, new AnnouncementEqualityComparer()))
                    {
                        announcements.Insert(iterator, newAnnouncement);
                        iterator++;
                    }
                }


            }
        }

        public static int GetModuleIndex(DataStructure.Module module)
        {
            return modules.IndexOf(module);
        }
        
        public static int GetModuleIndexByModuleId(string moduleId)
        {
            int index = 0;
            while (modules[index].moduleId != moduleId)
            {
                index++;
            }

            return index;
        }

        public static int GetAnnouncementIndex(int moduleIndex, String announcementId)
        {
            int index = 0;
            while (modules[moduleIndex].moduleAnnouncements[index].announceID != announcementId)
            {
                index++;
            }
            return index;
        }

        public static DataStructure.Module GetModuleAt(int index)
        {
            return modules[index];
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
            if (searchables.Count == 0)
            {
                foreach (DataStructure.Module module in Utils.DataManager.modules)
                {
                    searchables.Add(new DataStructure.Searchable(module.moduleCode,
                                                                 module.moduleName,
                                                                 module.moduleId,
                                                                 null));

                    foreach (DataStructure.Announcement announcement in module.moduleAnnouncements)
                    {
                        searchables.Add(new DataStructure.Searchable(announcement.announceName,
                                                                     announcement.announceContent,
                                                                     announcement.announceModuleId,
                                                                     announcement.announceID));
                    }
                }
            }
        }
    }

    class AnnouncementEqualityComparer : IEqualityComparer<DataStructure.Announcement>
    {
        public bool Equals(DataStructure.Announcement a1, DataStructure.Announcement a2)
        {
            if (a1.announceID == a2.announceID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(DataStructure.Announcement announcement)
        {
            int hCode = announcement.announceName.Length ^ announcement.announceContent.Length;
            return hCode.GetHashCode();
        }
    }

    class ModuleEqualityComparer : IEqualityComparer<DataStructure.Module>
    {
        public bool Equals(DataStructure.Module m1, DataStructure.Module m2)
        {
            if (m1.moduleId == m2.moduleId)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(DataStructure.Module mod)
        {
            int hCode = mod.moduleCode.Length ^ mod.moduleName.Length;
            return hCode.GetHashCode();
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
