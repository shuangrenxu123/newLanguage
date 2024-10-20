﻿using LoxLanguage.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoxLanguage
{

    /// <summary>
    /// 解释器
    /// </summary>
    internal class Interpreter : Stmt.IVisitor<Unit>,Expr.IVisitor<Object>
    {

        private Environment environment = new();
        public void Interpret(List<Stmt> statements)
        {
            try
            {
                foreach (Stmt stmt in statements)
                {
                    Execute(stmt);
                }
            }
            catch (RuntimeError ex)
            {
                Program.Error(ex.token.line,ex.Message);
                Program.hasRuntimeError = true;
            }

        }
        /// <summary>
        /// 执行语句
        /// </summary>
        /// <param name="stmt"></param>
        private void Execute(Stmt stmt)
        {
            stmt.Accept(this);
        }

        public object VisitBinaryExpr(Binary expr)
        {
            var left = Evaluate(expr.left);
            var right = Evaluate(expr.right);

            if (expr.opt.type == TokenType.Minus)
            {
                checkNumberOperand(expr.opt,left,right);
                return (float)left - (float)right;
            }

            if (expr.opt.type == TokenType.Plus)
            {
                if ((left is float) && (right is float))
                    return (float)left + (float)right;
                
                else if (left is string && right is float)
                    return (string)left + right.ToString();
                
                else if (left is float && right is string)
                    return ((float)left).ToString() + (string)right;

                else if (left is string && right is string)
                    return (string)left + (string)right;

                else
                {
                    throw new RuntimeError(expr.opt, "操作类型错误");
                }
            }

            if (expr.opt.type == TokenType.Star)
            {
                checkNumberOperand(expr.opt, left, right);
                return (float)left * (float)right;
            }
            if (expr.opt.type == TokenType.Slash)
            {
                checkNumberOperand(expr.opt, left, right);
                return (float)left / (float)right;
            }

            switch (expr.opt.type)
            {                
                case TokenType.Greater:
                    checkNumberOperand(expr.opt, left, right);
                    return (float)left > (float)right;
                case TokenType.Greater_Equal:
                    checkNumberOperand(expr.opt, left, right);
                    return (float)left >= (float)right;
                case TokenType.Less:
                    checkNumberOperand(expr.opt, left, right);
                    return (float)left < (float)right;
                case TokenType.Less_Equal:
                    checkNumberOperand(expr.opt, left, right);
                    return (float)left <= (float)right;
            }

            if(expr.opt.type == TokenType.Bang_Equal)
            {
                return !IsEqual(left, right);
            }
            if(expr.opt.type == TokenType.Equal_Equal)
            {
                return IsEqual(left, right);
            }

            return null;
        }

        public object VisitGroupingExpr(Grouping expr)
        {
            return Evaluate(expr.expression);
        }

        public object VisitLiteralExpr(Literal expr)
        {
            return expr.value;
        }

        public object VisitUnaryExpr(Unary expr)
        {
            object result = Evaluate(expr.right);

            if (expr.opt.type == TokenType.Minus)
            {
                return -(float)result;
            }

            if(expr.opt.type == TokenType.Bang)
            {
                return !IsTruthy(result);
            }

            return null;
        }

        private void checkNumberOperand(Token opt, params Object[] objs)
        {
            foreach (var i in objs)
            {
                if (i is not float)
                    throw new RuntimeError(opt, "类型错误");
            }
        }
        private bool IsEqual(object left,object right)
        {
            if(left == null && right == null)
                return true;
            if (left == null || right == null)
                return false;
            return left.Equals(right);
        }

        private bool IsTruthy(object value)
        {
            if (value == null)
                return false;
            if(value is bool)
            {
                return (bool)value;
            }
            return true;
        }

        /// <summary>
        /// 执行表达式
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        private Object Evaluate(Expr expr)
        {
            return expr.Accept(this);
        }

        public Unit VisitExpressionStmt(Expression stmt)
        {
            Evaluate(stmt.expression);
            return null;
        }

        public Unit VisitPrintStmt(Print stmt)
        {
            Object value = Evaluate(stmt.expression);
            Console.WriteLine(value);
            return null;
        }

        public Unit VisitVarStmt(Var stmt)
        {
            object value = null;
            if (stmt.initializer != null)
            {
                value = Evaluate(stmt.initializer);
            }

                environment.Define(stmt.name.lexeme,value);
                return null;
        }
        /// <summary>
        /// 变量表达式
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public object VisitVariableExpr(Variable expr)
        {
            return environment.GetVariables(expr.name);
        }

        /// <summary>
        /// 赋值语句的执行
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public object VisitAssignExpr(Assign expr)
        {
            object value = Evaluate(expr.right);
            environment.Assign(expr.name,value);
            //将最右侧的值传递过去
            return value;
        }

        public Unit VisitBlockStmt(Block stmt)
        {
            ExecuteBlock(stmt.statements,new Environment(environment));
            return null;
        }
        private void ExecuteBlock(List<Stmt> statements,Environment environment)
        {
            Environment previous = this.environment;
            try
            {
                //新的子环境
                this.environment = environment;
                foreach (Stmt stmt in statements)
                {
                    Execute(stmt);
                }
            }
            finally
            {
                //恢复环境
                this.environment = previous;
            }
        }
    } 
}
