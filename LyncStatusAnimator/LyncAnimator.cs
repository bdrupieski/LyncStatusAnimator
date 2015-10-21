using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Lync.Model;
using MoreLinq;

namespace LyncStatusAnimator
{
    public class LyncAnimator
    {
        private readonly LyncClient _client;
        private readonly Func<bool> _shouldCancel;
        private readonly Action<string> _reportNewStatus;
        private readonly IEnumerable<IEnumerable<string>> _normalAnimations; 
        private readonly IEnumerable<string> _happyFridayAnimation; 

        public LyncAnimator(Func<bool> shouldCancel, Action<string> reportNewStatus)
        {
            _client = LyncClient.GetClient();
            _shouldCancel = shouldCancel;
            _reportNewStatus = reportNewStatus;

            _normalAnimations = new[]
            {
                Wave(),
                BoomBox(),
                Fish(),
                Singing(),
                PingPong(),
            };

            _happyFridayAnimation = ScrollTextRightToLeft("Happy Friday!", 25);
        }

        public void Animate()
        {
            while (!_shouldCancel())
            {
                foreach (var animation in _normalAnimations)
                {
                    DisplayAnimation(animation);
                }

                if (DateTime.Today.DayOfWeek == DayOfWeek.Friday)
                {
                    DisplayAnimation(_happyFridayAnimation);
                }
            }

            PublishPersonalNote(string.Empty);
        }

        private void DisplayAnimation(IEnumerable<string> animation, int delayMilliseconds = 1000)
        {
            if (!_shouldCancel())
            {
                foreach (var frame in animation)
                {
                    PublishPersonalNote(frame);
                    Thread.Sleep(delayMilliseconds);

                    if (_shouldCancel())
                    {
                        break;
                    }
                }
            }
        }

        private void PublishPersonalNote(string newNote)
        {
            var publishData = new Dictionary<PublishableContactInformationType, object>
            {
                { PublishableContactInformationType.PersonalNote, newNote }
            };

            _client.Self.BeginPublishContactInformation(publishData, ar => _client.Self.EndPublishContactInformation(ar), null);
            _reportNewStatus(newNote);
        }

        private static IEnumerable<string> BoomBox()
        {
            return new[]
            {
                "♫♪.ılılıll|̲̅̅●̲̅̅|̲̅̅=̲̅̅|̲̅̅●̲̅̅|llılılı.♫♪",
                "♪♫.ılılıll|̲̅̅●̲̅̅|̲̅̅=̲̅̅|̲̅̅●̲̅̅|llılılı.♪♫",
                "♫♪.ılılıll|̲̅̅●̲̅̅|̲̅̅=̲̅̅|̲̅̅●̲̅̅|llılılı.♫♪",
                "♪♫.ılılıll|̲̅̅●̲̅̅|̲̅̅=̲̅̅|̲̅̅●̲̅̅|llılılı.♪♫",

                "♫♪.lılılıı|̲̅̅●̲̅̅|̲̅̅=̲̅̅|̲̅̅●̲̅̅|ıılılıl.♫♪",
                "♫♪.ılılıll|̲̅̅●̲̅̅|̲̅̅=̲̅̅|̲̅̅●̲̅̅|llılılı.♫♪",
                "♫♪.lılılıı|̲̅̅●̲̅̅|̲̅̅=̲̅̅|̲̅̅●̲̅̅|ıılılıl.♫♪",
                "♫♪.ılılıll|̲̅̅●̲̅̅|̲̅̅=̲̅̅|̲̅̅●̲̅̅|llılılı.♫♪",

                "♫♪.lılılıı|̲̅̅●̲̅̅|̲̅̅=̲̅̅|̲̅̅●̲̅̅|ıılılıl.♫♪",
                "♪♫.ılılıll|̲̅̅●̲̅̅|̲̅̅=̲̅̅|̲̅̅●̲̅̅|llılılı.♪♫",
                "♫♪.lılılıı|̲̅̅●̲̅̅|̲̅̅=̲̅̅|̲̅̅●̲̅̅|ıılılıl.♫♪",
                "♪♫.ılılıll|̲̅̅●̲̅̅|̲̅̅=̲̅̅|̲̅̅●̲̅̅|llılılı.♪♫",
            };
        }

        private static IEnumerable<string> Fish()
        {
            var fish = "><(((º>";
            var trail = "¸.·´¯`·.¸.·´¯`·.¸.·´¯`·.¸.·´";

            var fishAnimation = new List<string>();
            for (int i = 0; i < trail.Length; i++)
            {
                var fishFrame = new string(trail.Take(i).ToArray()) + fish;
                fishAnimation.Add(fishFrame);
            }

            return fishAnimation;
        }

        private static IEnumerable<string> Singing()
        {
            var personSinging = "d(^o^)b";
            var music = "¸¸♬·¯·♩¸¸♪·¯·♫¸¸♬·¯·♩¸¸♪·¯·♫¸¸";
            return music.Window(16).Reverse().Select(x => personSinging + new string(x.ToArray())).ToArray();
        }

        private static IEnumerable<string> PingPong()
        {
            //   ( '_')0*´¯`·.¸.·´¯`°Q('_' )

            return new[]
            {
                "( '_')0*            Q('_' )",
                "( '_')0 ´           Q('_' )",
                "( '_')0  ¯          Q('_' )",
                "( '_')0   `         Q('_' )",
                "( '_')0    ·        Q('_' )",
                "( '_')0     .       Q('_' )",
                "( '_')0      ¸      Q('_' )",
                "( '_')0       .     Q('_' )",
                "( '_')0        ·    Q('_' )",
                "( '_')0         ´   Q('_' )",
                "( '_')0          ¯  Q('_' )",
                "( '_')0           ` Q('_' )",
                "( '_')0            °Q('_' )",
            };
        }

        private static IEnumerable<string> Wave()
        {
            string wave = "°º¤ø,¸,ø¤º°`°º¤ø,¸,ø¤°º¤ø,¸,ø¤º°`°º¤ø,¸,ø¤º°`°º¤ø,¸,ø¤º°`°º¤ø,¸";
            return wave.Window(25).Select(x => new string(x.ToArray())).ToArray();
        }

        private static IEnumerable<string> ScrollTextRightToLeft(string msg, int windowSize)
        {
            var windowSizeInSpaces = "".PadLeft(windowSize, '\u2007');
            var paddedMsg = windowSizeInSpaces + msg + windowSizeInSpaces;

            var numSlidingWindows = windowSize + msg.Length + 1;
            
            var scrollingNotes = new List<string>();
            for (int i = 0; i < numSlidingWindows; i++)
            {
                var note = new string(paddedMsg.Skip(i).Take(windowSize).ToArray());
                scrollingNotes.Add(note);
            }
            return scrollingNotes;
        }
    }
}