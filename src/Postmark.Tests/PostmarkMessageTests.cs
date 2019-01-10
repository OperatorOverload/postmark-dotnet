﻿using PostmarkDotNet;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;

namespace Postmark.Tests
{
    public class PostmarkMessageTests
    {
        string singleFromAddress = "test-from@postmark.net";
        string singleToAddress = "atest-to@example.com";
        string singleCcAddress = "atest-cc@example.com";
        string singleBccAddress = "atest-bcc@example.com";

        [Fact]
        public void BasicMessageSetupWithCollectionValidation()
        {
            var message = new PostmarkMessage()
            {
                From = singleFromAddress,
                To = singleToAddress,
                Cc = singleCcAddress,
                Bcc = singleBccAddress,
                Subject = "Basic message setup",
                HtmlBody = "This is <b>HTML</b> text.",
                TextBody = "This is plain text.",
                TrackOpens = true,
                TrackLinks = LinkTrackingOptions.HtmlAndText,
                Tag = "message-setup-testing"
            };

            Assert.Equal(singleToAddress, message.To);
            Assert.Equal(singleToAddress, message.ToAddressSet.Single());

            Assert.Equal(singleCcAddress, message.Cc);
            Assert.Equal(singleCcAddress, message.CcAddressSet.Single());

            Assert.Equal(singleBccAddress, message.Bcc);
            Assert.Equal(singleBccAddress, message.BccAddressSet.Single());

        }

        [Fact]
        public void MessageSetupWithAddressAdditions()
        {
            var message = new PostmarkMessage()
            {
                From = singleFromAddress,
                To = singleToAddress,
                Cc = singleCcAddress,
                Bcc = singleBccAddress,
                Subject = "Basic message setup",
                HtmlBody = "This is <b>HTML</b> text.",
                TextBody = "This is plain text.",
                TrackOpens = true,
                TrackLinks = LinkTrackingOptions.HtmlAndText,
                Tag = "message-setup-testing"
            };

            string addressAdd = "test-addAddress@example.com";
            string addressAddDirty = "test-addAddress@example.com   ";

            //Test for To
            message.ToAddressSet.Add(addressAdd);
            Assert.NotEqual(singleToAddress, message.To);
            Assert.Contains(singleToAddress, message.To);
            Assert.Contains(addressAdd, message.To);

            //This is not safe as HashSet in the Framework does not guarantee ordering of output, replaced with contains below
            //Assert.Equal(singleToAddress + "," + addressAdd, message.To);

            message.To = addressAddDirty;
            Assert.Equal(addressAdd, message.To);
            
            message.ToAddressSet.Clear();
            message.ToAddressSet.Add(addressAddDirty);
            message.ToAddressSet.Add(singleToAddress);

            //This is not safe as HashSet in the Framework does not guarantee ordering of output, replaced with contains below
            //Assert.Equal(addressAdd + "," + singleToAddress, message.To);

            Assert.DoesNotContain(addressAddDirty, message.To);
            Assert.Contains(addressAdd, message.To);

            //Test for CC
            message.CcAddressSet.Add(addressAdd);
            Assert.NotEqual(singleCcAddress, message.Cc);

            //This is not safe as HashSet in the Framework does not guarantee ordering of output, replaced with contains below
            //Assert.Equal(singleCcAddress + "," + addressAdd, message.Cc);
            Assert.Contains(singleCcAddress, message.Cc);
            Assert.Contains(addressAdd, message.Cc);



            //Test for BCC
            message.BccAddressSet.Add(addressAdd);
            Assert.NotEqual(singleBccAddress, message.Bcc);
            
            //This is not safe as HashSet in the Framework does not guarantee ordering of output, replaced with contains below
            //Assert.Equal(singleBccAddress + "," + addressAdd, message.Bcc);

            Assert.Contains(singleBccAddress, message.Bcc);
            Assert.Contains(addressAdd, message.Bcc);
            
        }

        [Fact]
        public void MessageSetupWithAddressRemoval()
        {
            var message = new PostmarkMessage()
            {
                From = singleFromAddress,
                To = singleToAddress,
                Cc = singleCcAddress,
                Bcc = singleBccAddress,
                Subject = "Basic message setup",
                HtmlBody = "This is <b>HTML</b> text.",
                TextBody = "This is plain text.",
                TrackOpens = true,
                TrackLinks = LinkTrackingOptions.HtmlAndText,
                Tag = "message-setup-testing"
            };

            string addressAdd = "test-addAddress@example.com";
            string addressAdd2 = "test-addAddress2@example.com";

            message.ToAddressSet.Add(addressAdd);
            message.ToAddressSet.Add(addressAdd2);

            message.ToAddressSet.Remove(singleToAddress);

            Assert.Contains(addressAdd, message.To);
            Assert.Contains(addressAdd2, message.To);
            Assert.DoesNotContain(singleToAddress, message.To);
            
        }

        [Fact]
        public void MessageSetupWithEmptyAddressAddition()
        {
            var message = new PostmarkMessage()
            {
                From = singleFromAddress,
                To = singleToAddress,
                Cc = singleCcAddress,
                Bcc = singleBccAddress,
                Subject = "Basic message setup",
                HtmlBody = "This is <b>HTML</b> text.",
                TextBody = "This is plain text.",
                TrackOpens = true,
                TrackLinks = LinkTrackingOptions.HtmlAndText,
                Tag = "message-setup-testing"
            };

            string addressAdd = "test-addAddress@example.com";
            string addressAdd2 = "    ";
            string addressAdd3 = "test-addAddress2@example.com";

            message.ToAddressSet.Add(addressAdd);
            message.ToAddressSet.Add(addressAdd2);
            message.ToAddressSet.Add(addressAdd3);

            Assert.Contains(addressAdd, message.To);
            Assert.Contains(addressAdd3, message.To);

            //Message should not contain the blank space
            Assert.DoesNotContain(addressAdd2, message.To);

            //Message should not contain multiple commas with no address between them
            Assert.DoesNotContain(",,", message.To);

        }

    }
}