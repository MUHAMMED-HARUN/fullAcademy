# Home Dashboard — Academic Plan Cards (Frontend)

## 1. العيوب/الوضع الحالي
- `HomeView.vue` يعرض نموذج إضافة طالب (UpsertStudentComp) كصفحة رئيسية، غير مناسبة كلوحة معلومات.
- لا يوجد عرض للأ plans الأكاديمية في الشاشة الرئيسية.
- لا توجد تخصيص حسب الدور (Admin/Teacher/Student).

## 2. مقترح حسب الدور (توسيع)
### Admin
- KPI cards: عدد الخطط، نشطة، قيد التفعيل، معاهد، برامج، شعب.
- Academic Plan Cards (grid) مع status badge، scope، version، معهد، تواريخ.
- "Quick Actions": إنشاء خطة، تفعيل خطة، إدارة المستخدمين/الصلاحيات.
- "Recent Activity": آخر خطط، آخر شعب، آخر تسجيلات.
- (اختياري) charts: خطط حسب المعهد/البرنامج.

### Teacher
- "My Teaching" cards: مجموعات/مواد/شعب يدرسها.
- "Current Stage" progress card per group (stage name + progress %).
- Academic Plans المرتبطة (read-only) + اختصارات: رصد حضور، تقييم يومي، ترشيحات.
- "Today's Classes" (إن توفرت بيانات sessions) + تنبيهات (خطط جديدة، طلاب جدد).

### Student
- "My Academic Plan" card: version، معهد، programme، تقدم (progress ring).
- "Current Stage" card: stage name، subjects/matters، progress.
- "Attendance Summary" (present/absent/excused).
- "Recent Evaluations" list.
- "Upcoming/Next Stage" preview.

### Common
- Welcome header (name + role + date).
- Notifications/announcements panel.
- Search quick (plans, students, groups).

## 3. خطة Frontend (UI/UX)
- إنشاء مكون `AcademicPlanCardGrid.vue` (أو إعادة استخدام `AcademicPlanCard`) بتصميم Tailwind cards (responsive grid).
- تعديل `HomeView.vue`:
  - إزالة/نقل UpsertStudentComp (أو إبقاؤه في قسم "إجراءات سريعة" للأدمن).
  - إضافة `v-if` حسب `authStore.UserRole`.
  - Admin/Teacher: جلب AcademicPlans عبر `academicPlanStore.fetchAcademicPlans({ Page:1, PageSize: 6/12 })`.
  - Student: جلب StudentAcademicEnrollment (خطته) — reuse store.
- إضافة skeleton/loading و empty state.
- RTL + Main.css identity.

## 4. خطة Backend (إن لزم)
- لا تغييرات required؛ `GetAcademicPlanList` موجود.
- (اختياري) endpoint مختصر "dashboard summary" (counts) — not required now.

## 5. توجيهات المستخدم
- "استعرض الأخطط الأكاديمية على شكل كاردز احترافية" → grid of cards.
- "حسب كل دور" → role-based sections.

## 6. خطوات التنفيذ لاحقاً
1. إنشاء `AcademicPlanDashboardCard.vue` (card design).
2. تعديل HomeView.
3. Wire Pinia store.
4. Test build.
