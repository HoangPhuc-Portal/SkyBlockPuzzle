using System;using System.Reflection;using NUnit.Framework;
public sealed class GameOverTests {
 [Test] public void RecordIsStrictAndResetPreservesHighScore(){
 var type=Type.GetType("GameOverChecks, Assembly-CSharp-Editor",true);
 try{var result=(string)type.GetMethod("RunScoreAndLock").Invoke(null,null);StringAssert.StartsWith("PASS:",result);TestContext.WriteLine(result);}
 catch(TargetInvocationException error){throw error.InnerException??error;}
 }
}