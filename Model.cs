using System.Collections.Generic;
using JsonTableAttribute;

namespace NestedReflection
{
    public class MyInfo
    {
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }

        [TableName("EducationInfo")]
        public List<EducationInfo> Nested { get; set; } = new List<EducationInfo>();

        [TableName("Family")]
        public List<Family> familyInfo  {get; set;} = new List<Family>();
    }

    public class EducationInfo
    {
        public string ExamName { get; set; }
        public string Year { get; set; }
        public string Board { get; set; }
        public string Result { get; set; }
        
    }

    public class Family
    {
        public string Name { get; set; }
        public string Relation { get; set; }
        public int Age { get; set; }
        public string Occupation { get; set; }

        [InnerTableName("FamilyBackground")]
        public List<FamilyBackground> familyBackground {get; set;} = new List<FamilyBackground>();
    }

     public class FamilyBackground
    {
        public string PresentAddress { get; set; }
        public string PresentAddress2 { get; set; }
    }
}   