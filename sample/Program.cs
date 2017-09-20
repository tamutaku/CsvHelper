using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zip;

public class Program
{
	public static void Main(string[] args)
	{
		var people = new List<Person>();
		people.Add(new Person() { Name = "太郎", Kana = "タロウ", BirthDate = new DateTime(1980, 1, 1) });
		people.Add(new Person() { Name = "次郎", Kana = "ジロウ", BirthDate = new DateTime(1990, 2, 2) });
		people.Add(new Person() { Name = "三郎", Kana = "サブロウ", BirthDate = new DateTime(2000, 3, 3) });

        using (StringWriter sw = new StringWriter())
		using (CsvWriter csvWriter = new CsvWriter(sw))
		{
			csvWriter.Configuration.HasHeaderRecord = true;
            csvWriter.Configuration.Delimiter = "\t";
			csvWriter.Configuration.RegisterClassMap<PersonMapper>();
			csvWriter.WriteRecords(people);

            byte[] csv = System.Text.Encoding.UTF8.GetBytes(sw.ToString());
            MemoryStream ms = new MemoryStream(csv);

			using (ZipFile zip = new ZipFile())
			{
				zip.Password = "12345";
                zip.UpdateEntry("people.csv", ms);
				zip.Save("all.zip");
			}
		}
	}

    public class Person
	{
		public string Name { get; set; }
		public string Kana { get; set; }
		public DateTime BirthDate { get; set; }
	}

	public class PersonMapper : CsvClassMap<Person>
	{
		public PersonMapper()
		{
			Map(x => x.Name).Index(0).Name("氏名");
			Map(x => x.BirthDate).Index(1).Name("生年月日").TypeConverterOption("yyyy/MM/dd");
			Map(x => x.Kana).Ignore();
		}
	}

}