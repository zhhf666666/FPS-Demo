using System.Collections.Generic;
using System.Text;


public static class JSONConvert
{
    #region Global Variables
    private static char[] _charary;
    private static int _aryend;

    #endregion

    #region JSON Deserialization

    // Convert string to JSONObject
    private static JSONObject DeserializeSingletonObject(ref int left)
    {
        JSONObject localjson = new JSONObject();
        while (left <= _aryend)
        {
            char c = _charary[left];
            if (c == ' ' || c == '\r' || c == '\n' || c == '\t')  //skip empty char
            {
                left++;
                continue;
            }
            if (c == ',')
            {
                left++;
                continue;
            }
            char r = '\0';
            if (c == '\"' || c == '\'')     //beginning of key
            {
                left++;
                r = c;
            }
            else if (c == '}')      //end of JSONObject
            {
                left++;
                break;
            }
            int column = left;
            if (r == '\0')
            {
                while (_charary[column] != ':') column++;
            }
            else
            {
                while (!(_charary[column] == r && _charary[column - 1] != '\\' && _charary[column + 1] == ':')) column++;
            }
            string key = new string(_charary, left, column - left);         //get the key
            if (r == '\0')
                left = column + 1;
            else
                left = column + 2;
            c = _charary[left];
            while (c == ' ' || c == '\r' || c == '\n' || c == '\t')  //skip empty char
            {
                left++;
                c = _charary[left];
            }
            if (c == '\"' || c == '\'')     //if value is string
            {
                left++;
                int strend = left;
                while (_charary[strend] != c || _charary[strend - 1] == '\\') strend++;
                localjson[key] = new string(_charary, left, strend - left);
                left = strend + 1;
            }
            else if (c == '{') // JSONObject
            {
                left++;
                localjson[key] = DeserializeSingletonObject(ref left);
            }
            else if (c == '[')     //JSONArray
            {
                left++;
                localjson[key] = DeserializeSingletonArray(ref left);
            }
            else
            {
                //other class, such as boolean, int
                //all are converted to string, it can be enriched if in need
                int comma = left;
                char co = _charary[comma];
                while (co != ',' && co != '}')
                {
                    comma++;
                    co = _charary[comma];
                }
                int em = comma - 1;
                co = _charary[em];
                while (co == ' ' || co == '\r' || co == '\n' || co == '\t')
                {
                    em--;
                    co = _charary[em];
                }
                localjson[key] = new string(_charary, left, em - left + 1);
                left = comma;
            }
        }
        return localjson;
    }

    // Convert string to JSONArray
    private static JSONArray DeserializeSingletonArray(ref int left)
    {
        JSONArray jsary = new JSONArray();
        while (left <= _aryend)
        {
            char c = _charary[left];
            if (c == ' ' || c == '\r' || c == '\n' || c == '\t')  //skip empty char
            {
                left++;
                continue;
            }
            if (c == ',')
            {
                left++;
                continue;
            }
            if (c == ']')
            {
                left++;
                break;
            }
            if (c == '{') //JSONObject
            {
                left++;
                jsary.Add(DeserializeSingletonObject(ref left));
            }
            else if (c == '[')     //JSONArray
            {
                left++;
                jsary.Add(DeserializeSingletonArray(ref left));
            }
            else if (c == '\"' || c == '\'')            //string
            {
                left++;
                int strend = left;
                while (_charary[strend] != c || _charary[strend - 1] == '\\') strend++;
                jsary.Add(new string(_charary, left, strend - left));
                left = strend + 1;
            }
            else
            {
                //other class, such as boolean, int
                //all are converted to string, it can be enriched if in need
                int comma = left;
                char co = _charary[comma];
                while (co != ',' && co != ']')
                {
                    comma++;
                    co = _charary[comma];
                }
                int em = comma - 1;
                co = _charary[em];
                while (co == ' ' || co == '\r' || co == '\n' || co == '\t')
                {
                    em--;
                    co = _charary[em];
                }
                jsary.Add(new string(_charary, left, em - left + 1));
                left = comma;
            }
        }
        return jsary;
    }

    #endregion

    #region Public Interface

    // Get a JSONObject instance from char[]
    public static JSONObject DeserializeCharToObject(char[] input)
    {
        _charary = input;
        _aryend = _charary.Length - 1;
        while (_aryend > 0)
            if (_charary[_aryend] != '}')
                _aryend--;
            else
                break;
        int start = 0;
        while (start < _aryend)
            if (_charary[start] != '{')
                start++;
            else
                break;
        start++;
        if (_aryend < start + 1)
            return null;
        return DeserializeSingletonObject(ref start);
    }

    // Get a JSONObject instance from string
    public static JSONObject DeserializeObject(string input)
    {
        return DeserializeCharToObject(input.ToCharArray());     //The first char must be '{'
    }

    // Get a JSONArray instance from char[]
    public static JSONArray DeserializeCharsToArray(char[] input)
    {
        _charary = input;
        _aryend = _charary.Length - 1;
        while (_aryend > 0)
            if (_charary[_aryend] != ']')
                _aryend--;
            else
                break;
        int start = 0;
        while (start < _aryend)
            if (_charary[start] != '[')
                start++;
            else
                break;
        start++;
        if (_aryend < start + 1)
            return null;
        return DeserializeSingletonArray(ref start);
    }

    // Get a JSONArray instance from string
    public static JSONArray DeserializeArray(string input)
    {
        return DeserializeCharsToArray(input.ToCharArray());
    }

    // Serialize a JSONObject instance
    public static string SerializeObject(JSONObject jsonObject)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("{");
        foreach (KeyValuePair<string, object> kvp in jsonObject)
        {
            if (kvp.Value is JSONObject)
            {
                sb.Append(string.Format("\"{0}\":{1},", kvp.Key, SerializeObject((JSONObject)kvp.Value)));
            }
            else if (kvp.Value is JSONArray)
            {
                sb.Append(string.Format("\"{0}\":{1},", kvp.Key, SerializeArray((JSONArray)kvp.Value)));
            }
            else if (kvp.Value is string)
            {
                sb.Append(string.Format("\"{0}\":\"{1}\",", kvp.Key, kvp.Value));
            }
            else if (kvp.Value is int || kvp.Value is long)
            {
                sb.Append(string.Format("\"{0}\":{1},", kvp.Key, kvp.Value));
            }
            else
            {
                sb.Append(string.Format("\"{0}\":\"{1}\",", kvp.Key, ""));
            }
        }
        if (sb.Length > 1)
            sb.Remove(sb.Length - 1, 1);
        sb.Append("}");
        return sb.ToString();
    }

    // Serialize a JSONArray instance
    public static string SerializeArray(JSONArray jsonArray)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("[");
        for (int i = 0; i < jsonArray.Count; i++)
        {
            if (jsonArray[i] is JSONObject)
            {
                sb.Append(string.Format("{0},", SerializeObject((JSONObject)jsonArray[i])));
            }
            else if (jsonArray[i] is JSONArray)
            {
                sb.Append(string.Format("{0},", SerializeArray((JSONArray)jsonArray[i])));
            }
            else if (jsonArray[i] is string)
            {
                sb.Append(string.Format("\"{0}\",", jsonArray[i]));
            }
            else
            {
                sb.Append(string.Format("\"{0}\",", ""));
            }

        }
        if (sb.Length > 1)
            sb.Remove(sb.Length - 1, 1);
        sb.Append("]");
        return sb.ToString();
    }

    #endregion
}


public class JSONObject : Dictionary<string, object>
{
    public void put(string key, string value)
    {
        this[key] = value;
    }

    public void put(string key, int value)
    {
        this[key] = value.ToString();
    }
}


public class JSONArray : List<object>
{ }