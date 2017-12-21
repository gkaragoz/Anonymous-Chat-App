using System;
using System.Collections.Generic;
using System.Linq;

namespace ARW.SRP{

	public class SpecialRequestParam
	{

		private IDictionary<string, object> dataList;

		public SpecialRequestParam (){
			this.dataList = new Dictionary<string, object> ();
		}


		public void PutVariable(string key, object value){
			this.dataList.Add (key, value);
		}

		public string GetString(string key){

			try{
				var entry = dataList.Where (a => a.Key == key).Select (a => (KeyValuePair<string,object>?) a).FirstOrDefault ();
				return entry.Value.Value.ToString();
			}catch(System.NullReferenceException e){
				Console.WriteLine ("There was nothing like " + key);
			}catch(InvalidOperationException){}

			return string.Empty;	
		}

		public bool GetBool(string key){

			try{
				var entry = dataList.Where (a => a.Key == key).Select (a => (KeyValuePair<string,object>?) a).FirstOrDefault ();
				return bool.Parse(entry.Value.Value.ToString());
			}catch(System.NullReferenceException e){
				Console.WriteLine ("There was nothing like " + key);
			}catch(InvalidOperationException){}

			return false;
		}

		public int GetInt(string key){
			var entry = dataList.Where (a => a.Key == key).Select (a => (KeyValuePair<string,object>?) a).FirstOrDefault ();

			try{
				return int.Parse(entry.Value.Value.ToString());
			}catch(System.NullReferenceException e){
				Console.WriteLine ("There was nothing like " + key);
			}catch(InvalidOperationException){}

			return 0;
		}

		public float GetFloat(string key){
			var entry = dataList.Where (a => a.Key == key).Select (a => (KeyValuePair<string,object>?) a).FirstOrDefault ();

			try{
				return float.Parse(entry.Value.Value.ToString());
			}catch(System.NullReferenceException e){
				Console.WriteLine ("There was nothing like " + key);
			}catch(InvalidOperationException){}

			return 0.0f;
		}

		public string Compress(){
			string data = string.Empty;

			foreach (KeyValuePair<string, object> p in dataList) {
				data += p.Key + "#=#" + p.Value + "###";
			}
			if(data.Length >= 3 && data.Substring(data.Length-3) == "###"){
				data = data.Substring(0, data.Length-3);
			}

			return data;
		}

		public static SpecialRequestParam Extract(string data){
			SpecialRequestParam newSpecialEventParam = new SpecialRequestParam();

			if (data == null)
				return newSpecialEventParam;

			string[] variables = data.Split (new string[]{"###"}, StringSplitOptions.None);

			foreach (string variable in variables) {
				string[] varParts = variable.Split (new string[]{"#=#"}, StringSplitOptions.None);
				if (varParts.Length == 2)
					newSpecialEventParam.dataList.Add (varParts [0], varParts [1]);
			}

			return newSpecialEventParam;
		}
	}
}