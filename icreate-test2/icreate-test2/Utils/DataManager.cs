using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace icreate_test2.Utils
{
    static class DataManager
    {
        private static bool isLoaded;
        private static List<DataStructure.Module> _modules;
        private static List<DataStructure.Announcement> _announcements;
        private static List<DataStructure.Class> _classes;
        private static List<DataStructure.SemesterInfo> _sems;

        public static void InitializeDataLists()
        {
            _modules = new List<DataStructure.Module>();
            _announcements = new List<DataStructure.Announcement>();
            _classes = new List<DataStructure.Class>();
            _sems = new List<DataStructure.SemesterInfo>();

            isLoaded = false;
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
            return _classes;
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
            _classes.Add(newClass);
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



}
