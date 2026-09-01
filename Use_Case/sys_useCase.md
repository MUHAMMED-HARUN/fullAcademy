# نمط التوثيق
	شرح 
	الاعمدة
	طريقة انشائه
	شروط
	العلاقات
# 
هبكلية البرنامج
	Clean Architecture
	sql Database
	Ef Core orm
	WebApi
	Vue Frontend


# 
شروط ال Exception 
	## Domain
		تعمل داخل ال Domain وتستخدم عدة كيانات هي
			DomainException هو المسؤول عن اطلاق الخطاء
			DomainExceptionObj هو المسؤول عن بيانات الخطا
			DomainExeptionMsg هو المكان المسؤول عن تعبئة DomainExceptionObj كي يسهل تتبع رسائل الخطا وتغييرها مع الوقت
	Application
		شرح وطريقة العمل
		هذه الطبقة لا تصدر أي خطء انما تقوم بامساك الأخطاء من ال Domain ومن ال Infrastructure
		لا تقوم بإعادة اصدار Exception  انما ال TResult هو المسؤول عن هذه الخطوة
			اذا حدث أي خطا يقوم بتعيين isSeccuss=false  ويقوم بوضع رسالة الخطأ في ال ErrorMessage 
		شرط 
			لا تقوم بارسال الأخطاء الصادرة عن ال Domain   و ال Infrastructure  مباشر ولكن تقوم بتعبئة ال TResult  من ال ApplicationErrorMsg  
			وال ApplicationErrorMsg يقوم بانشاء خطا لكل حالة وتستدعى منه وقت الحاجة
		ترحيل البيانات 
			من ال DTO الى ال Entity دائما عير  Mapper
			