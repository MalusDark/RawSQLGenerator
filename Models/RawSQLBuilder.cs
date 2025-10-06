using RawSQLGenerator.Enums;
using RawSQLGenerator.Helpers;
using System.Drawing;
using System.Text;

namespace RawSQLGenerator.Models;
/// <summary>
/// This is the main class that converts your lists into a raw Sql query.
/// </summary>
public class RawSQLBuilder
{
	/// <summary>
	/// Method for generating a ONLY select query based on your dictionary of the form «table name - column name» 
	/// and aliases as « { 0, "Users.Id = UserId" } ».
	/// </summary>
	/// <param name="tablesWithColumns"></param>
	/// <returns></returns>
	public static string OnlySelect(Dictionary<string, List<string>> tablesWithColumns, 
		List<JoinTypes> joins = null,
		Dictionary<int, string> tableAliases = null)
	{
		var allColumns = new List<string>();
		var sql = new StringBuilder();
		sql.Append("SELECT ");

		foreach (var table in tablesWithColumns)
		{
			foreach (var column in table.Value)
			{
				allColumns.Add($"{table.Key}.{column}");
			}
		}

		sql.Append(string.Join(", ", allColumns));
		sql.Append(" FROM ");
		sql.Append(string.Join(", ", tablesWithColumns.Keys));

		if (joins != null)
		{ 
			for (var i = 0; i < joins.Count; i++)
			{
				sql.Append($" {SQLHelper.GetJoinTypeString(joins[i])}");
				if (tableAliases != null && tableAliases.ContainsKey(i))
				{
					sql.Append($" ON {tableAliases.Where(a => a.Key.Equals(i)).Select(a => a.Value).First()}");
				}
			}
		}

		return sql.ToString();
	}
}
