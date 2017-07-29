namespace Project.TblFaculty {
    public class Faculty {
        private string id;
        private string name;
        private int? year;

        public Faculty() {
        }

        public Faculty(string id, string name, int? year) {
            this.id = id;
            this.name = name;
            this.year = year;
        }

        public Faculty(string id, string name) {
            this.id = id;
            this.name = name;
        }

        public string Id {
            get { return id; }
            set { id = value; }
        }

        public string Name {
            get { return name; }
            set { name = value; }
        }

        public int? Year {
            get { return year; }
            set { year = value; }
        }

        public override string ToString() {
            return base.ToString();
        }

        public override bool Equals(object obj) {
            return base.Equals(obj);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}
