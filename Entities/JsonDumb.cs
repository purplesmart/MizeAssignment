namespace Entities;

    public class JsonDumb : ICloneable
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public JsonDumb(string name, int age)
        {
            Name = name;
            Age = age;
        }

    public object Clone()
    {
        return new JsonDumb(Name,Age);
    }
}
