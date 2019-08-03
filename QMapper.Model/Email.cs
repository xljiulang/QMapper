using System;
using System.Collections.Generic;
using System.Text;

namespace QMapper.Model
{
    public class Email
    {
        private readonly string value;

        public Email(string value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return this.value;
        }

        public static explicit operator string(Email email)
        {
            return email?.ToString();
        }

        public static explicit operator Email(string str)
        {
            return new Email(str);
        }
    }
}
