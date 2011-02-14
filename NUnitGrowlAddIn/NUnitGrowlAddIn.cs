using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Growl.Connector;
using Growl.CoreLibrary;
using NUnit.Core;
using NUnit.Core.Extensibility;
using NUnitGrowlAddIn.Properties;


/*
 * copyright (c) Antonio Lucca 2010.
 * _do_what_you_want_ license
 * tonyx1@gmail.com
 * 
 */

namespace NUnitGrowlAddIn
{

    [NUnitAddin(Type = (NUnit.Core.Extensibility.ExtensionType)1, Name = "NUnit", Description = "NUnit Add-in that sends test results to Growl.")]
    public sealed class NUnitGrowlAddIn : IAddin, EventListener
    {

        private const string _testRunFailed = "NUnit.TestRun.Failed";
        private const string _testRunFirstTestFailed = "NUnit.TestRun.FirstTestFailed";
        private const string _testRunStarted = "NUnit.TestRun.Started";
        private const string _testRunSucceeded = "NUnit.TestRun.Succeeded";

        private int successCounter;
        private int failureCounter;
        private int errorCounter;

        private readonly Application growlApplication;
        private readonly GrowlConnector growlConnector;


        public NUnitGrowlAddIn()
        {
                        
            growlConnector = new GrowlConnector();

            if (growlConnector.IsGrowlRunning())
            {

                growlApplication = new Application("NUnit");
                NotificationType[] notificationTypeArr1 = new NotificationType[] {
                                                                                   new NotificationType("NUnit.TestRun.Started"), 
                                                                                   new NotificationType("NUnit.TestRun.Succeeded"), 
                                                                                   new NotificationType("NUnit.TestRun.Failed"), 
                                                                                   new NotificationType("NUnit.TestRun.FirstTestFailed"),
                                                                                   new NotificationType("NUnit.TestRun.Exception")

                };
                
                NotificationType[] notificationTypes = notificationTypeArr1;
                growlConnector.Register(growlApplication, notificationTypes);

            }
        }

        public bool Install(IExtensionHost host)
        {

            IExtensionPoint extensionPoint = host.GetExtensionPoint("EventListeners");
            if (extensionPoint != null)
            {
                extensionPoint.Install(this);
            }
          
            return false;
        }

        private void NotifyFirstTestFailure(TestResult result)
        {


        }

        private void NotifyTestRunFailure(TestResult result)
        {
        	
         	        	        	
        	  string message =  result.FullName+"\n";    
        	  
            message+="test executed: "+result.Test.TestCount+"\n";
            message+="test time: "+result.Time+"\n";
            message+= "failed : " + this.failureCounter+"\n";
            message += "error : " + this.errorCounter + "\n";
            message += "success: "+this.successCounter+"\n";
        	 string name = "";
            Bitmap image;
            name = "NUnit.TestRun.Failed";
            image = Resources.thumbupred;
        	 string messageId = NUnitGrowlAddIn.GetNewMessageId();
            Notification notification = new Notification("NUnit", name, messageId, "NUnit test run.", message, image, false, Priority.Normal, messageId);
            growlConnector.Notify(notification);
          
        }

        private void NotifyTestRunStarted(string name, int testCount)
        {

            
        }

        private void NotifyTestRunSuccess(TestResult result)
        {


        }

        public void RunFinished(TestResult result)
        {
        	

			try {

            string message = result.FullName +"\n";  
            
            message+="test executed: "+result.Test.TestCount+"\n";
            message+="test time: "+result.Time+"\n";
            message+= "failed : " + this.failureCounter+"\n";
            message += "error : " + this.errorCounter + "\n";
            message += "success: "+this.successCounter+"\n";


            
            string messageId = NUnitGrowlAddIn.GetNewMessageId();
            string name = "";
            Bitmap image;

            if (result.IsSuccess)
            {

                name = "NUnit.TestRun.Succeeded";
                image = Resources.thumbup;
                
                Notification notification = new Notification("NUnit", name, messageId, "NUnit test run.", message, image, false, Priority.Normal, messageId);
                growlConnector.Notify(notification);
		
                
            } 
            
//            else
//            {
//                name = "NUnit.TestRun.Failed";
//                image = Resources.thumbupred;
//            }
//
//            Notification notification = new Notification("NUnit", name, messageId, "NUnit test run.", message, image, false, Priority.Normal, messageId);
//            growlConnector.Notify(notification);
			} catch (Exception e)
			{
        		string messageId  = GetNewMessageId();
        		
        		Notification notification = new Notification("NUnit", "NUnit.TestRun.Exception", messageId, "NUnit addin test internal error", e.Message, null, false, Priority.High, messageId);
            		
			}

        }



        public void RunFinished(Exception exception)
        {
            Bitmap image = Resources.thumbupred;
            string messageId = NUnitGrowlAddIn.GetNewMessageId();

            Notification notification = new Notification("NUnit", "NUnit.Test.Failed", messageId, "NUnit test run.", exception.Message, image, false, Priority.High, messageId);
            growlConnector.Notify(notification);
          
        }

        public void RunStarted(string name, int testCount)
        {
        	
        }

        public void SuiteFinished(TestResult result)
        {
        }

        public void SuiteStarted(TestName testName)
        {

            this.successCounter = 0;
            this.failureCounter = 0;
            this.errorCounter = 0;

        }

        public void TestFinished(TestResult result)
        {

        	try {
        		
        		if (result.IsSuccess)
        		{
        			this.successCounter++;
        		}

        		if (result.IsFailure)
        		{
        			this.failureCounter++;
        		}

        		if (result.IsError)
        		{
        			this.errorCounter++;
        		}
        		if (!result.IsSuccess)
        		{
        			
        			string message = result.FullName;

        			string resmessage = result.Message;
        			string messageId = NUnitGrowlAddIn.GetNewMessageId();
        			Bitmap bmp = null;
        			
        			Notification notification = new Notification("NUnit", "NUnit.TestRun.Failed", messageId, "NUnit test error.", message + " " + resmessage, Properties.Resources.thumbupred, false, Priority.Normal, messageId);

        			growlConnector.Notify(notification);
//
        		}
        	} catch (Exception e)
        	{
        		string messageId  = GetNewMessageId();        		
        		Notification notification = new Notification("NUnit", "NUnit.TestRun.Exception", messageId, "NUnit addin test internal error", e.Message, null, false, Priority.High, messageId);            		
        		
        	}

        	
        }

        public void TestOutput(TestOutput testOutput)
        {
        }

        public void TestStarted(TestName testName)
        {
        }

        public void UnhandledException(Exception exception)
        {
        	

        }

        private static string GetNewMessageId()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString();
        }

    } 

}

