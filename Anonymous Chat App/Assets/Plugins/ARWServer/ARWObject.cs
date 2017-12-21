using System;
using System.Collections.Generic;
using System.Linq;

namespace ARW.Com
{

	public class IARWObject : ARWObject
	{

		private IDictionary<string, object> _dataList;
		public IDictionary<string, object> dataList{
			set{
				_dataList = value;
			}
			get{
				return _dataList;
			}
		}

		public IARWObject(){
			dataList 		= new Dictionary<string, object> ();
		}

		public void PutString(string key, string value){
			dataList.Add (key, value);	
		}

		public void PutBool(string key, bool value){
			dataList.Add (key, value);	
		}

		public void PutInt(string key, int value){
			dataList.Add (key, value);	
		}

		public void PutFloat(string key, float value){
			dataList.Add (key, value);
		}

		public string GetString(string key){
			var entry = dataList.Where (a => a.Key == key).Select (a => (KeyValuePair<string,object>?) a).FirstOrDefault ();

			try{
				return entry.Value.Value.ToString();
			}catch(System.NullReferenceException e){
				Console.WriteLine ("There was nothing like " + key);
			}catch(System.InvalidOperationException e){}

			return string.Empty;
		}

		public bool GetBool(string key){
			var entry = dataList.Where (a => a.Key == key).Select (a => (KeyValuePair<string,object>?) a).FirstOrDefault ();

			try{
				return bool.Parse(entry.Value.Value.ToString());
			}catch(System.NullReferenceException e){
				Console.WriteLine ("There was nothing like " + key);
			}

			return false;
		}

		public int GetInt(string key){
			var entry = dataList.Where (a => a.Key == key).Select (a => (KeyValuePair<string,object>?) a).FirstOrDefault ();

			try{
				return int.Parse(entry.Value.Value.ToString());
			}catch(System.NullReferenceException e){
				Console.WriteLine ("There was nothing like " + key);
			}

			return 0;
		}

		public float GetFloat(string key){
			var entry = dataList.Where (a => a.Key == key).Select (a => (KeyValuePair<string,object>?) a).FirstOrDefault ();

			try{
				return float.Parse(entry.Value.Value.ToString());
			}catch(System.NullReferenceException e){
				Console.WriteLine ("There was nothing like " + key);
			}

			return 0.0f;
		}

		public static ARWObject Extract(byte[] bytes){
			string data = System.Text.Encoding.UTF8.GetString (bytes).Replace("\0", null).Replace("\"",null);

			ARWObject newObj = IARWObject.Extract(data);
			return newObj;
		}

		public static ARWObject Extract(string data){
			ARWObject newObj = new IARWObject ();
			string[] prms = data.Split (new string[]{"###"}, StringSplitOptions.None);
			foreach (string p in prms) {
				string[] paramParts = p.Split (new string[]{"#=#"}, StringSplitOptions.None);
				if (paramParts.Length == 2)
					newObj.dataList.Add (paramParts [0], paramParts [1]);
			}
			return newObj;
		}

		public byte[] Compress(){
			string data = String.Empty;

			foreach (KeyValuePair<string, object> p in dataList) {
				data += p.Key + "#=#" + p.Value + "###";
			}
			if(data.Length >= 3 && data.Substring(data.Length-3) == "###"){
				data = data.Substring(0, data.Length-3);
			}

			byte[] bytes = System.Text.Encoding.UTF8.GetBytes (data);
			return bytes;
		}
	}

	public interface ARWObject{
		IDictionary<string, object> dataList{
			set;	get;
		}
		void PutString(string key, string value);
		void PutInt(string key, int value);
		void PutFloat(string key, float value);
		string GetString(string key);
		int GetInt(string key);
		float GetFloat(string key);
		byte[] Compress();
	}
}