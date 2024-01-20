using System;

namespace DefaultSetting
{
    public enum Category
    {
        Red,
        Green,
        Blue,
    }

    [Serializable]
    public class ExImporterEntity
    {
        public uint id;
        public string name;
        public int price;
        public bool isNotForSale;
        public float rate;
        public Category category;

        public ExImporterEntity DeepCopy()
        {
            ExImporterEntity newCopy = new ExImporterEntity();

            //newCopy.id = this.id;
            //newCopy.stage = this.stage;
            //newCopy.coinCount = this.coinCount;
            //...데이터 넣기

            return newCopy;
        }
    }
}