using RawSQLGenerator.Enums;

namespace RawSQLGenerator.Helpers;
public static class SQLHelper
{
	public static string GetJoinTypeString(JoinTypes joinType)
	{
		return joinType switch
		{
			JoinTypes.Inner => "INNER JOIN",
			JoinTypes.Left => "LEFT JOIN",
			JoinTypes.Right => "RIGHT JOIN",
			JoinTypes.Full => "FULL JOIN",
			_ => "JOIN"
		};
	}
}
