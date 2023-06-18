using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Text.RegularExpressions;
using System;
using ACS.Infrastructure;

namespace ACS.Model
{
    [Table(name: "pList")]
    public class Person : ICloneable
    {
        [Column("ID")]
        public int ID { get; set; }
        [Column("Name")]
        private string? _Name;
        public string? Name
        {
            get => _Name != null ? Regex.Replace(_Name, "\\d+_", "") : null;
            set => _Name = value;
        }
        [Column("FirstName")]
        public string? FirstName { get; set; }
        [Column("MidName")]
        public string? MidName { get; set; }
        [Column("TabNumber")]
        public string? TabNumber { get; set; }
        [Column("Post")]
        public int PostId { get; set; }
        public Post? Post { get; set; }
        [Column("Section")]
        public int DivisionId { get; set; }
        public Division? Division { get; }
        [Column("Company")]
        public int? CompanyId { get; set; }


        private byte[]? _BytePicture;
        [Column("Picture", TypeName = "image")]
        public byte[]? BytePicture
        {
            get => _BytePicture?.Length > 10 ? _BytePicture : NoFoto.NoFotoByte;
            set => _BytePicture = value;

        }

        public Company? Company { get; set; }
        [NotMapped]
        public string FullName
        {
            get => Name + " " + FirstName + " " + MidName;
        }

        public Person()
        {

        }
        public Person(Person p)
        {
            this.BytePicture = p.BytePicture?.Length != 0 ? p.BytePicture : NoFoto.NoFotoByte;
            this.Company = p.Company;
            this.FirstName = p.FirstName;
            this.TabNumber = p.TabNumber;
            this.PostId = p.PostId;
            this.Post = p.Post;
            this.Division = p.Division;
            this.DivisionId = p.DivisionId;
            this.CompanyId = p.CompanyId;
            this.Name = p.Name;
            this.MidName = p.MidName;
            this.ID = p.ID;

        }
        public object Clone()
        {
            return new Person(this);
        }
    }
}
