namespace RandomNameGenerator
{
    internal class JpnName
    {
        private string personalRomaji, personalKanji, gender, familyRomaji, familyKanji;

        public JpnName()
        {
            this.personalRomaji = "Tarou";
            this.personalKanji = "太郎";
            this.gender = "male";
            this.familyRomaji = "Yamada";
            this.familyKanji = "山田";
        }

        public JpnName(string newPRomaji, string newPKanji, string newGender, string newFRomaji, string newFKanji)
        {
            this.personalRomaji = newPRomaji;
            this.personalKanji = newPKanji;
            this.gender = newGender;
            this.familyRomaji = newFRomaji;
            this.familyKanji = newFKanji;
        }

        public string getPersonalRomaji()
        {
            return this.personalRomaji;
        }

        public void setPersonalRomaji(string newName)
        {
            this.personalRomaji = newName;
        }

        public string getPersonalKanji()
        {
            return this.personalKanji;
        }

        public void setPersonalKanji(string newName)
        {
            this.personalKanji = newName;
        }

        public string getGender()
        {
            return this.gender;
        }

        public void setGender(string newGender)
        {
            this.gender = newGender;
        }

        public string getFamilyRomaji()
        {
            return this.familyRomaji;
        }

        public void setFamilyRomaji(string newName)
        {
            this.familyRomaji = newName;
        }

        public string getFamilyKanji()
        {
            return this.familyKanji;
        }

        public void setFamilyKanji(string newName)
        {
            this.familyKanji = newName;
        }

        public string getWesternName()
        {
            return (this.personalRomaji + " " + this.familyRomaji);
        }

        public string getEasternName()
        {
            return (this.familyKanji + this.personalKanji);
        }

        public override string ToString()
        {
            string newString = "";
            newString += this.getWesternName();
            newString += " ( ";
            newString += this.getEasternName();
            newString += " ) (";
            newString += this.getGender();
            newString += ")";
            return newString;
        }
    }
}