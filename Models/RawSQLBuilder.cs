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
	/// Method for generating a select query based on your dictionary of the form «table name - column name» 
	/// and aliases as « { 0, "Users.Id = UserId" } ».
	/// </summary>
	/// <param name="tablesWithColumns"></param>
	/// <returns></returns>
	public static string MainSelect(Dictionary<string, List<string>> tablesWithColumns, 
		List<JoinTypes> joins = null,
		Dictionary<int, string> tableAliases = null,
		bool distinct = false,
		int? limit = null,
		Dictionary<string, OrderTypes> orderBy = null,
		List<string> groupBy = null)
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
		if (orderBy != null)
		{
			var count = 0;
			sql.Append(" ORDER BY ");
			foreach (var orderAtribute  in orderBy)
			{
				sql.Append(orderAtribute.Key);
				sql.Append(" ");
				sql.Append(orderAtribute.Value);
				if (count < orderBy.Count - 1)
					sql.Append(", ");
				count++;
			}
		}
		if (orderBy != null)
		{
			var count = 0;
			sql.Append(" GROUP BY ");
			foreach (var groupAtribute in groupBy)
			{
				sql.Append(groupAtribute); 
				if (count < groupBy.Count - 1)
					sql.Append(", ");
				count++;
			}
		}
		if (distinct) sql.Append(" DISTINCT");
		if (limit.HasValue) sql.Append($" LIMIT {limit} ");

		return sql.ToString();
	}

	public static string MainUpdate(Dictionary<string, List<string>> tablesWithColumns)
	{
		var sql = new StringBuilder();
		sql.Append("UPDATE ");
		sql.Append(tablesWithColumns.FirstOrDefault().Key);
		sql.Append(" SET");
		
		var columns = tablesWithColumns.Select(c => c.Value).First();
		var count = 0;
		foreach (var column in columns)
		{
			sql.Append($" {column}");
			if (count < columns.Count - 1)
				sql.Append(',');
			count++;
		}

		return sql.ToString();
	}
}
