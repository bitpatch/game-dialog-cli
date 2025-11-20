using System.Collections.Generic;

namespace BitPatch.DialogLang
{
    static class Utils
    {
        public static void PushStatements(this Stack<Ast.Statement> stack, IReadOnlyList<Ast.Statement> statements)
        {
            for (int i = statements.Count - 1; i >= 0; i--)
            {
                stack.Push(statements[i]);
            }
        }

        public static Location After(this Location location)
        {
            return new Location(location.Line, location.Final, location.Final + 1);
        }
    }
}