using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

namespace pacmangame
{
	[Serializable]
	public class ClassScoreManager
	{
		// класс "счет"
		[Serializable]
		public class ScoreItem
		{
			public int Value = 0;
		}

		public ScoreItem Score;

		// прочитать счет обоих режимов из файла
		public ClassScoreManager ReadScores()
		{
			try
			{
				var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
				var filePath = System.IO.Path.Combine(sdCardPath+"/Application/Dageron Studio", "dageron_angry_pacman.xml");
				FileStream fStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
				var myBinaryFormatter = new BinaryFormatter();
				var mc = (ClassScoreManager) myBinaryFormatter.Deserialize(fStream);
				fStream.Close();
				return mc;
			}
			catch (Exception e)
			{
				Score = new ScoreItem ();
				return this;
			}
		}

		// записать счет обоих режимов в файл
		public void WriteScores()
		{
			var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
			if (!Directory.Exists(sdCardPath +"/Application")) Directory.CreateDirectory (sdCardPath +"/Application");
			if (!Directory.Exists(sdCardPath +"/Application/Dageron Studio")) Directory.CreateDirectory (sdCardPath +"/Application/Dageron Studio");
			var filePath = System.IO.Path.Combine(sdCardPath+"/Application/Dageron Studio", "dageron_angry_pacman.xml");
			FileStream fStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
			var myBinaryFormatter = new BinaryFormatter();
			myBinaryFormatter.Serialize(fStream, this);
			fStream.Close();
		}
	}
}

