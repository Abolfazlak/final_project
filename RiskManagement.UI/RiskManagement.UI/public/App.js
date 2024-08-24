import "./App.css";
import logo from "./assets/Logo.PNG";
import identify from "./assets/Identify.JPG";
import Analys from "./assets/Analys.PNG";
import Plan from "./assets/Plan.PNG";
import Track from "./assets/Track.PNG";
import Control from "./assets/Control.PNG";
import Diagram from "./assets/Diagram.PNG";

function App() {
  return (
    <div className="Landing">
      <div className="Landing_Banner">
        <img src={logo} alt="logo" className="Landing_Banner_Logo" />
        <div className="Landing_Banner_Column">
          <h1 className="Landing_Banner_Title">
            سامانه جامع مدیریت ریسک‌های نرم‌افزاری راسا
          </h1>
          <h3 dir="rtl" className="Landing_Banner_Text">
            سامانه مدیریت ریسک پروژه‌خهای نرم‌افزاری راسا، ابزاریست برای
            شناسایی، بررسی، برنامه‌ریزی، پیگیری و کنترل ریسک‌ها در جهت مدیریت و
            کاهش تاثیر آنها بر پروژه
          </h3>
        </div>
      </div>
      <div className="Landing_table">
        <h2 className="Landing_table_title">ویژگی‌های راسا</h2>
        <div className="Landing_table_cart white_color">قابلیت شخصی‌سازی و <br/>دوری از اکتفا به سوالات از پیش‌تعیین‌شده</div>
        <div className="Landing_table_cart white_color">ارائه آمار و ارقام به <br/> صورت نمودار‌های قابل فهم</div>
        <div className="Landing_table_cart white_color">قابلیت تجزیه و تحلیل دقیق<br/> ریسک‌های نرم‌افزاری</div>
      </div>
      <div className="Landing_table">
        <div className="Landing_table_cart white_color">کمک‌رسانی به تیم مدیریتی در جهت<br/> اتخاذ تصمیم‌های بهتر و دقیق‌تر</div>
        <div className="Landing_table_cart white_color">ایجاد بستر مناسب جهت پیگیری <br/>راه حل‌های مدنظر جهت کنترل ریسک‌های موجود</div>
        <div className="Landing_table_cart white_color">در نظر گرفتن محدودیت‌های زمانی و ددلاین ها و محدود کردن مدت زمان اجرای راه حل‌ها</div>
        <h2 className="Landing_table_title white_color">ویژگی‌های راسا</h2>
      </div>
<div className="Landing_table Landing_diagram_justify">
  <img src={Diagram} alt="Diagram" className="Landing_diagram"/>
  <h2 className="Landing_diagram_title">نمودار اجزای مدیریت ریسک <br/>در پروژه‌های نرم‌افزاری</h2>
</div>
      <div className="Landing_Explanation">
        <img
          src={identify}
          alt="identify"
          className="Landing_Explanation_picture"
        />
        <p dir="rtl" className="Landing_Explanation_Text">
         مقصود از شناسایی ریسک‌های پروژه‌های نرم افزاری، کشف تمامی موانع و مشکلاتی است که می تواند به صورت مستقیم یا غیرمستقیم منجر به شکست یا ایجاد اختلال در فرآیند پیشرفت پروژه شوند. ما با شناسایی ریسک‌های موجود یا احتمالی قدم مهمی در جهت مدیریت ریسک‌های پروژه نرم افزاری برمیداریم.
        </p>
        <div className="Landing_Case"> شناسایی</div>
      </div>
      <div className="Landing_Explanation">
        <div className="Landing_Case">تجزیه و تحلیل</div>
        <p dir="rtl" className="Landing_Explanation_Text">
         در گام تجزیه و تحلیل لازم است موارد مختلف نظیر اولویت ریسک و میزان خطرمندی هر ریسک را بررسی نماییم. پس در واقع این گام زمینه لازم برای اجرای فعالیت‌های مدیریت ریسک را فراهم می‌کند. 
        </p>
        <img
          src={Analys}
          alt="Analys"
          className="Landing_Explanation_picture"
        />
      </div>
      <div className="Landing_Explanation">
        <img src={Plan} alt="Plan" className="Landing_Explanation_picture" />
        <p dir="rtl" className="Landing_Explanation_Text">
          پس آنالیز، اطلاعات مورد نیاز از هر ریسک در اختیار تیم مدیریتی قرار می‌گیرد. اهمیت مدیریت ریسک در پیشرفت پروژه‌های نرم‌افزاری ایجاب می‌کند که در گام بعدی واکنش مناسب با هر ریسک انتخاب و طرح کلی برای مقابله با آن انتخاب گردد. 
        </p>
        <div className="Landing_Case">برنامه‌ریزی</div>
      </div>
      <div className="Landing_Explanation">
        <div className="Landing_Case">پیگیری</div>
        <p dir="rtl" className="Landing_Explanation_Text">
          لازمه هر سامانه مدیریتی موفق، قابلیت مدیریت و پیگیری برنامه‌های منتخب در گام قبل و اطمینان از صحت عملکرد و نتیجه‌بخش بودن آنهاست. بنابراین گام چهارم از نمودار اجزای مدیریت ریسک به پیگیری یا به کلام دیگر مدیرت برنامه‌ها اختصاص یافته است.
        </p>
        <img src={Track} alt="Track" className="Landing_Explanation_picture" />
      </div>
      <div className="Landing_Explanation">
        <img
          src={Control}
          alt="Control"
          className="Landing_Explanation_picture"
        />
        <p dir="rtl" className="Landing_Explanation_Text">
      در گام آخر سعی بر کنترل ریسک های شناسایی شده از طریق برنامه‌ها و راه‌حل‌های ارائه شده، می‌باشد. این گام در واقع هدف نهایی از فرایند مدیریت ریسک است. 
        </p>
        <div className="Landing_Case">کنترل</div>
      </div>
    </div>
  );
}

export default App;
