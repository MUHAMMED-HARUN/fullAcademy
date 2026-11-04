---
name: backend-infrastructure-persistence
description: Implement EF Core configurations, AppDbContext registration, feature repository services, dependency injection, and Unit of Work behavior in School_Academy.Infrastructure.
---

# البنية التحتية وPersistence

استخدم هذه المهارة عند إضافة أو تعديل mapping في EF Core، أو مستودع، أو تسجيل Infrastructure، أو تكامل مع Unit of Work.

## قواعد إلزامية

- استخدم مهارة `backend-security-scoping` إلزامياً عند بناء أو تعديل أي Repository/ميزة لتطبيق نطاق الملكية المركزي على استعلامات القراءة، مع الحفاظ على فلتر الحذف الناعم.
- سياق قاعدة البيانات الفعلي اسمه `AppDbContext` في `System.Infrastructure/Persistence`. لا تنشئ سياقاً موازياً باسم آخر.
- Configuration الكيان تكون في `Configurations/{Feature}`، باسم `{Entity}Configuration`، وترث `BaseEntityConfiguration<TEntity>` للكيانات الموروثة من `clsBaseClass`.
- طابق namespace مع المسار، وراجع المفاتيح الخارجية وخصائص navigation معاً. لا تكرر إعداد علاقة قديمة خاطئة لمجرد أنها موجودة.
- أضف `DbSet<TEntity>` إلى `AppDbContext` عندما يلزم، واحتفظ باكتشاف الإعدادات عبر `ApplyConfigurationsFromAssembly`. راجع مرشحات الحذف المنطقي وإعدادات Identity والعروض عند تأثرها.
- عقد المستودع `I*Repository` يُعرّف في `System.Application/Abstractions/{Feature}`؛ تنفيذه في Infrastructure يسمى `*Service`، مثلاً `StageService : IStageRepository`.
- استخدم `AppDbContext`/`DbSet` للوصول للبيانات. المستودع يضيف أو يحدّث tracked entities؛ الحفظ والمعاملة مسؤولية `IUnitOfWork` والـ pipeline، فلا تضف `SaveChanges` متفرقاً داخل كل Repository.
- سجّل كل تنفيذ جديد في `InfrastructureServiceRegistration` بنمط DI الملائم للمشروع (`AddScoped`). تحقق من العقد الفعلي والـ namespace قبل التسجيل.
- أوامر `ICommand` العادية تحفظ عبر `TransactionBehavior`، أما `ITransaction` فيدخل مسار المعاملة ونشر الأحداث. راجع `TransactionBehavior` و`AppDbContext.SaveChangesAsync` قبل لمس نشر Domain Events لتجنب فقدها أو نشرها مرتين.
- استخدم `Persistence/Seeding` و`HasData` للبيانات الثابتة المناسبة فقط. لا تضع أسراراً أو بيانات تشغيلية متغيرة في Seed أو migration.
- لأي Lookup كبير، أضف عقد `GetListFilter` إلى `I*Repository` ونفّذه على `IQueryable` في `*Service` باستخدام `GridifyAsync` وإسقاط DTO قبل جلب النتائج؛ لا تستخدم `ToListAsync()` ثم تصفية الذاكرة. حافظ على PageSize محدوداً وعلى Global/soft-delete filters، ويُمنع استخدام `IgnoreQueryFilters()` في مسار Lookup.
- اجعل حقول Gridify المسموحة واضحة في الـ DTO/Mapping، ووفّر استعلام `GetById` أو `get-for-upsert` يعيد المعرفات والعلاقات اللازمة لوضع التعديل دون تحميل القوائم كاملة.

## سير العمل

1. اقرأ كيان Domain وعقد `I*Repository` وConfiguration وRepository قريباً.
2. نفّذ mapping والعلاقات بأسماء وحقول متطابقة مع Domain وDTO والعقد.
3. أضف التنفيذ إلى DI، ثم راجع تسجيل `AppDbContext` ومكان migrations إذا تغيّر المخطط.
4. عند مشاركة الميزة مع Application أو WebApi، اقرأ مهارات CQRS وWebApi ذات الصلة.

## التحقق قبل الإنهاء

- لا يوجد namespace يخالف مسار الملف ولا علاقة FK تشير إلى خاصية غير المقصودة.
- Repository implementer وinterface والتسجيل في DI متطابقة، ولا يجري الحفظ خارج المسار المعتمد.
- افحص migration أو build عند ملاءمة النطاق، وأبلغ عن أي أثر يتطلب ترحيلاً أو إعداداً خارج التغيير المطلوب.
