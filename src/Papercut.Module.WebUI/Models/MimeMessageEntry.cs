﻿// Papercut
//
// Copyright © 2008 - 2012 Ken Robertson
// Copyright © 2013 - 2017 Jaben Cargman
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.


namespace Papercut.Module.WebUI.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Core.Domain.Message;
    using MimeKit;

    public class MimeMessageEntry : MessageEntry
    {
        public string Subject => MailMessage?.Subject;

        public DateTime? Created => _created;

        public string Id => this.Name;

        public MimeMessage MailMessage { get; }

        public MimeMessageEntry(MessageEntry entry, MimeMessage message) : base(entry.File)
        {
            MailMessage = message;
        }

        public class RefDto
        {
            public static RefDto CreateFrom(MimeMessageEntry messageEntry)
            {
                return new RefDto
                {
                    Subject = messageEntry.Subject,
                    CreatedAt = messageEntry.Created?.ToUniversalTime(),
                    Id = messageEntry.Id,
                    Size = messageEntry.FileSize
                };
            }

            public string Size { get; set; }

            public string Id { get; set; }

            public DateTime? CreatedAt { get; set; }

            public string Subject { get; set; }
        }


        public class Dto
        {
            public static Dto CreateFrom(MimeMessageEntry messageEntry)
            {
                var mail = messageEntry.MailMessage;

                return new Dto
                {
                    Subject = messageEntry.Subject,
                    CreatedAt = messageEntry.Created?.ToUniversalTime(),
                    Id = messageEntry.Id,
                    From = ToAddressList(mail?.From),
                    To = ToAddressList(mail?.To),
                    Cc = ToAddressList(mail?.Cc),
                    BCc = ToAddressList(mail?.Bcc),
                    HtmlBody = mail?.HtmlBody,
                    TextBody = mail?.TextBody,
                };
            }

            static List<EmailAddressDto> ToAddressList(IList<InternetAddress> mailAddresses)
            {
                if (mailAddresses == null)
                {
                    return new List<EmailAddressDto>();
                }

                return mailAddresses
                    .OfType<MailboxAddress>()
                    .Select(f => new EmailAddressDto {Address = f.Address, Name = f.Name})
                    .ToList();
            }



            public string Id { get; set; }

            public DateTime? CreatedAt { get; set; }

            public string Subject { get; set; }


            public List<EmailAddressDto> From { get; set; } = new List<EmailAddressDto>();

            public List<EmailAddressDto> To { get; set; } = new List<EmailAddressDto>();

            public List<EmailAddressDto> Cc { get; set; } = new List<EmailAddressDto>();

            public List<EmailAddressDto> BCc { get; set; } = new List<EmailAddressDto>();


            public string HtmlBody { get; set; }
            
            public string TextBody { get; set; }

           
        }

        public class EmailAddressDto
        {
            public string Name { get; set; }
            public string Address { get; set; }
        }
    }
}