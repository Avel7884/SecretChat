using System;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text.RegularExpressions;
using NUnit.Framework;
using FakeItEasy;
using NUnit.Framework.Internal;
using SecretChat;

namespace SecretChatUnitTests
{
    [TestFixture]
    public class OneTimePadUnitTests
    {
        private TextReader reader;
        private TextWriter writer;
        private IKeyReader keyReader;
        
        [SetUp]
        protected void SetUp()
        {
            reader = A.Fake<TextReader>();
            writer = A.Fake<TextWriter>();
            keyReader = A.Fake<IKeyReader>();
        }

        private void StringArySame(string text)
        {
            var oneTimePad = new OneTimePasCryptoStream(reader, writer, keyReader);
            
            A.CallTo(() => reader.ReadLine())
                .Returns(text);
            oneTimePad.WriteMessage("", oneTimePad.ReadMessage());
            
            A.CallTo(() => writer.WriteLine(A<string>.That.Matches(s => s == text)))
                .MustHaveHappened(Repeated.Exactly.Once);   
        }

        [Test]
        public void TestSimpleEnglishText()
        {
            StringArySame("SimpleText");
        }
        
        [Test]
        public void TestSimpleTextWithSpacesAndMarks()
        {
            StringArySame("I'm very S!MPLE & clever text!!!..., Is'nt it???");
        }
        
        [Test]
        public void TestSimpleRussianText()
        {
            StringArySame("Просто текст");
        }
        
        [Test]
        public void TestRussianText()
        {
            StringArySame("Я просто текст с буквой ЁЁЁёёёё и алфавитом: " +
                          "йцукенгшщзфывапролджэхъячсмитьбюЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ");
        }
        
        [Test]
        public void TestAllRussianAbdEnglishSymbols()
        {
            StringArySame(
                "~!@#$%^&*()_+QWERTYUIOP{}|ASDFGHJKL:\"ZXCVBNM<>?`1234567890" +
                "-=qwertyuiop[]\\asdfghjkl);'zxcvbnm,./Ё!\"№);%:?*()_+ЙЦУКЕНГШЩ" +
                "ЗХЪ/ФЫВАПРОЛДЖЭЯЧСМИТЬБЮ,ё1234567890-=йцукенгшщзхъ\\фывапролджэячсмитьбю.");
        }

        [Test]
        public void TestLongText()
        {
            StringArySame(
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam scelerisque sollicitudin dui sed aliquam. Suspendisse dolor libero, ultrices laoreet mauris et, consequat ultrices nulla. Proin eget malesuada tortor. Cras maximus fermentum nulla, et dapibus justo. Nam ipsum massa, congue a pulvinar sed, tempus et velit. Suspendisse consequat orci placerat, elementum risus et, sollicitudin urna. Pellentesque lacus erat, viverra eu purus ut, dapibus posuere dui. Cras porttitor justo non facilisis finibus. Quisque eget libero nec turpis tempor scelerisque ut a leo. Nullam egestas eleifend est at porta. Fusce cursus, leo id hendrerit porttitor, sem massa ultricies sem, eget vestibulum augue ex non odio. Aenean luctus, magna id pulvinar varius, lacus libero lobortis lorem, eleifend pellentesque leo purus eget tellus. Etiam vulputate risus velit, et condimentum tellus convallis eget.\n\nProin tempor justo vel varius porttitor. Maecenas vulputate dui in mauris lobortis, at aliquet libero finibus. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Quisque gravida commodo risus, eu blandit nibh ornare non. Vestibulum quis lacinia neque, quis consequat mi. Sed tortor felis, sollicitudin vitae augue id, volutpat fermentum ante. Phasellus et consectetur augue. Phasellus arcu urna, tempus eget magna a, accumsan tempor arcu.\n\nFusce hendrerit ac quam vitae lacinia. Duis interdum nibh sit amet nunc lobortis interdum non nec justo. In in augue rutrum, dapibus sem eget, imperdiet diam. Nulla vehicula malesuada interdum. Aenean consectetur arcu eget cursus semper. Praesent tristique porttitor nisi nec luctus. Sed id ante varius, blandit orci vitae, maximus neque. Vestibulum condimentum, nibh id tempor luctus, massa est luctus nunc, quis facilisis turpis eros tempus lorem. Vestibulum diam magna, faucibus dictum lacinia eu, tincidunt vel metus. Duis nec lorem vel enim tristique dapibus.\n\nMorbi aliquet orci nec lacinia auctor. Praesent vitae dignissim est, vitae aliquam nulla. Sed vehicula, neque non semper rhoncus, purus neque luctus diam, ut commodo eros arcu sed justo. Sed eget convallis lectus. Donec eget sagittis mauris. Proin eget malesuada erat. Nullam consequat nunc tortor, nec placerat sapien iaculis gravida. Duis et accumsan augue. Suspendisse et massa ante. Proin eu aliquam dolor. Ut cursus arcu eros, in eleifend nulla imperdiet non. Aliquam ornare est a arcu commodo gravida sit amet at eros. Pellentesque ligula quam, fringilla in erat at, imperdiet imperdiet velit. Fusce id imperdiet mi. Nunc vitae ornare magna, id mattis odio.\n\nQuisque ac semper nunc. Mauris lorem purus, condimentum non vehicula eu, laoreet vel mauris. Sed at faucibus turpis. Cras in vestibulum tellus, at consequat dui. Integer sodales, quam et laoreet tristique, eros neque laoreet felis, ac efficitur tortor leo sit amet velit. Aliquam convallis odio ut lectus mattis, et facilisis ipsum sagittis. Nullam lobortis bibendum turpis. Vivamus eget erat suscipit, eleifend massa ut, iaculis orci. Pellentesque quis rhoncus tellus, eget tincidunt ex. Morbi porta finibus libero, sit amet euismod nulla tristique ac.\n"
                );
        }

        [Test]
        public void TestSimpleChineseText()
        {
            StringArySame(
                "側経意責家方家閉討店暖育田庁載社転線宇。得君新術治温抗添代話考振投員殴大闘北裁。品間識部案代学凰処済準世一戸刻法分。悼測済諏計飯利安凶断理資沢同岩面文認革。内警格化再薬方久化体教御決数詭芸得筆代。"
                );
        }

        [Test]
        public void TestJapaneseText()
        {
            StringArySame(
                "旅ロ京青利セムレ弱改フヨス波府かばぼ意送でぼ調掲察たス日西重ケアナ住橋ユムミク順待ふかんぼ人奨貯鏡すびそ。"
                );
        }

        [Test]
        public void TestArabText()
        {
            StringArySame(
                "غينيا واستمر العصبة ضرب قد. وباءت الأمريكي الأوربيين هو به،, هو العالم، الثقيلة بال. مع وايرلندا الأوروبيّون كان, قد بحق أسابيع العظمى واعتلاء. انه كل وإقامة المواد."
                );
        }
    }
}