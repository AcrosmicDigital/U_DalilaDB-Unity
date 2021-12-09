using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;


namespace U.DalilaDB
{

    [DataContract(Name = "TimeLapse")]
    public struct TimeLapse
    {
        [DataMember()]
        private uint seconds_;
        public uint seconds { 
            get 
            { 
                return seconds_; 
            } 
            set 
            {
                seconds_ = value;

                while (seconds_ >= 60)
                {
                    minutes++;
                    seconds_ -= 60;
                }
            } 
        }

        [DataMember()]
        private uint minutes_;
        public uint minutes
        {
            get
            {
                return minutes_;
            }
            set
            {
                minutes_ = value;

                while (minutes_ >= 60)
                {
                    hours++;
                    minutes_ -= 60;
                }
            }
        } // 60 seconds

        [DataMember()]
        private uint hours_;
        public uint hours
        {
            get
            {
                return hours_;
            }
            set
            {
                hours_ = value;

                while (hours_ >= 24)
                {
                    days++;
                    hours_ -= 24;
                }
            }
        } // 60 minutes

        [DataMember()]
        private uint days_;
        public uint days
        {
            get
            {
                return days_;
            }
            set
            {
                days_ = value;

                while (days_ >= 30)
                {
                    months++;
                    days_ -= 30;
                }
            }
        } // 24 hours

        [DataMember()]
        private uint months_;
        public uint months
        {
            get
            {
                return months_;
            }
            set
            {
                months_ = value;

                while (months_ >= 12)
                {
                    years++;
                    months_ -= 12;
                }
            }
        } // 30 days

        [DataMember()]
        private uint years_;
        public uint years
        {
            get
            {
                return years_;
            }
            set
            {
                years_ = value;
            }
        } // 12 months



        #region Comparator operators

        public override bool Equals(object obj)
        {
            if((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }

            TimeLapse tl = (TimeLapse)obj;

            if (
                tl.seconds == this.seconds &&
                tl.minutes == this.minutes &&
                tl.hours == this.hours &&
                tl.days == this.days &&
                tl.months == this.months &&
                tl.years == this.years
                ) return true;

            return false;
        }

        public static bool operator==(TimeLapse obj1, TimeLapse obj2)
        {
            var equals = true;

            try
            {
                equals = obj1.Equals(obj2);
            }
            catch (System.Exception)
            {
                try
                {
                    equals = obj2.Equals(obj1);
                }
                catch (System.Exception)
                {

                }
            }

            return equals;
        }

        public static bool operator!=(TimeLapse obj1, TimeLapse obj2)
        {
            return !(obj1 == obj2);
        }

        public static bool operator >(TimeLapse obj1, TimeLapse obj2)
        {

            if (obj1.years > obj2.years) return true;
            if (obj1.months > obj2.months) return true;
            if (obj1.days > obj2.days) return true;
            if (obj1.hours > obj2.hours) return true;
            if (obj1.minutes > obj2.minutes) return true;
            if (obj1.seconds > obj2.seconds) return true;
            return false;

        }

        public static bool operator <(TimeLapse obj1, TimeLapse obj2)
        {
            if (obj1.years < obj2.years) return true;
            if (obj1.months < obj2.months) return true;
            if (obj1.days < obj2.days) return true;
            if (obj1.hours < obj2.hours) return true;
            if (obj1.minutes < obj2.minutes) return true;
            if (obj1.seconds < obj2.seconds) return true;
            return false;
        }

        public static bool operator >=(TimeLapse obj1, TimeLapse obj2)
        {
            if (obj1 == obj2) return true;

            return (obj1 > obj2);
        }

        public static bool operator <=(TimeLapse obj1, TimeLapse obj2)
        {
            if (obj1 == obj2) return true;

            return (obj1 < obj2);
        }


        #endregion Comparator operators


        #region Cast operators

        public static implicit operator string(TimeLapse tl)
        {
            if (tl == null) return "aaaa:mm:dd:hh:mm:ss";

            return tl.years + ":" + tl.months + ":" + tl.days + ":" + tl.hours + ":" + tl.minutes + ":" + tl.seconds;
        }

        public override string ToString()
        {
            return this + "";
        }

        public static implicit operator TimeLapse(int seconds)
        {
            if (seconds < 0) seconds = 0;

            return new TimeLapse
            {
                seconds = (uint)seconds,
            };
        }

        public static implicit operator TimeLapse(float seconds)
        {
            if (seconds < 0) seconds = 0;

            return new TimeLapse
            {
                seconds = (uint)seconds,
            };
        }

        public static implicit operator uint(TimeLapse tl)
        {
            return
                tl.seconds +
                (tl.minutes * 60) +
                (tl.hours * 3600) +
                (tl.days * 86400) +
                (tl.months * 2592000)+
                (tl.years * 31104000);
        }

        public static implicit operator int(TimeLapse tl)
        {
            return
                (int)(tl.seconds +
                (tl.minutes * 60) +
                (tl.hours * 3600) +
                (tl.days * 86400) +
                (tl.months * 2592000) +
                (tl.years * 31104000));
        }

        #endregion Cast operators


        #region Aritmethic operators

        public static TimeLapse operator +(TimeLapse obj1, TimeLapse obj2)
        {
            TimeLapse newLapse = new TimeLapse();

            newLapse.seconds = obj1.seconds + obj2.seconds;
            newLapse.minutes += obj1.minutes + obj2.minutes;
            newLapse.hours += obj1.hours + obj2.hours;
            newLapse.days += obj1.days + obj2.days;
            newLapse.months += obj1.months + obj2.months;
            newLapse.years += obj1.years + obj2.years;

            return newLapse;
        }

        public static TimeLapse operator -(TimeLapse obj1, TimeLapse obj2)
        {

            var seconds = (int)obj1.seconds - (int)obj2.seconds;
            var minutes = (int)obj1.minutes - (int)obj2.minutes;
            var hours = (int)obj1.hours - (int)obj2.hours;
            var days = (int)obj1.days - (int)obj2.days;
            var months = (int)obj1.months - (int)obj2.months;
            var years = (int)obj1.years - (int)obj2.years;

            while (seconds < 0)
            {
                minutes--;
                seconds += 60;
            }

            while (minutes < 0)
            {
                hours--;
                minutes += 60;
            }

            while (hours < 0)
            {
                days--;
                hours += 24;
            }

            while (days < 0)
            {
                months--;
                days += 30;
            }

            while (months < 0)
            {
                years--;
                months += 12;
            }

            if (years < 0) return new TimeLapse();

            return new TimeLapse
            {
                seconds = (uint)seconds,
                minutes = (uint)minutes,
                hours = (uint)hours,
                days = (uint)days,
                months = (uint)months,
                years = (uint)years,
            };
        }

        #endregion Aritmethic operators



        public override int GetHashCode()
        {
            return (seconds + minutes + hours + days + months + years).GetHashCode();
        }



    }
}
