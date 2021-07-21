using System;

namespace RandomNameGenerator
{
    internal class JpnNameGenerator
    {
        private string[] femaleNames = System.IO.File.ReadAllLines("FemaleNames.txt");
        private string[] maleNames = System.IO.File.ReadAllLines("MaleNames.txt");
        private string[] neutralNames = System.IO.File.ReadAllLines("NeutralNames.txt");
        private string[] familyNames = System.IO.File.ReadAllLines("FamilyNames.txt");

        public string getRandomPersonalName(int choice = 0)
        {
            Random r = new Random();
            if (choice == 0)
            {
                choice = r.Next(1, 4);
            }

            if (choice == 1) //Male
            {
                int index = r.Next(0, maleNames.Length);
                string name = maleNames[index];
                return name + "/male";
            }
            else if (choice == 2) //Female
            {
                int index = r.Next(0, femaleNames.Length);
                string name = femaleNames[index];
                return name + "/female";
            }
            else //Neutral
            {
                int index = r.Next(0, neutralNames.Length);
                string name = neutralNames[index];
                return name + "/neutral";
            }
        }

        public string getRandomFamilyName()
        {
            Random r = new Random();
            int index = r.Next(0, familyNames.Length);
            string name = familyNames[index];
            return name;
        }

        public string[] getFormattedName(string oldName)
        {
            string[] newName = oldName.Split("/");
            if (newName[0].Contains(";"))
            {
                string[] randomName = newName[0].Split(';');
                Random r = new Random();
                int index = r.Next(0, randomName.Length);
                newName[0] = randomName[index];
            }
            return newName;
        }
    }
}