# NHA-4-169
Auto generated repo 169
....
# 🏠 BayTack

> **A trusted digital marketplace connecting customers with verified home maintenance service providers through secure payments, transparent bidding, and quality assurance.**

---

# 📖 Business Model

## 🎯 Vision

BayTack aims to become the trusted platform for home maintenance services by connecting customers with verified professionals in a secure, fast, and transparent environment.

---

# 🎯 Business Goals

- Simplify the entire customer journey:
  - Search for services
  - Create jobs
  - Receive bids
  - Select providers
  - Make secure payments
  - Leave ratings and reviews
- Provide reliable income opportunities for both individual professionals and maintenance companies.
- Improve service quality through transparent ratings and customer feedback.
- Securely manage payments and commission distribution.
- Deliver business insights through dashboards, reports, and KPIs.

---

# 👥 User Roles

## 👤 Customer

Customers can:

- Search for services
- Create maintenance jobs
- Receive and compare bids
- Select service providers
- Make secure payments
- Rate and review completed services

---

## 🛠️ Provider

BayTack supports two provider types:

- **Individual Provider**
- **Company Provider**

Providers can:

- Create professional profiles
- Upload verification (KYC) documents
- Build a portfolio
- Manage availability
- Submit bids
- Complete customer orders

---

## 👨‍💼 Admin

Administrators are responsible for:

- Managing platform users
- Reviewing provider verification
- Monitoring orders and disputes
- Managing payments and commissions
- Maintaining audit logs

---

# 💎 Value Proposition

## For Customers

- ✅ Verified service providers
- ✅ Competitive pricing through bidding
- ✅ Transparent ratings and reviews
- ✅ Direct communication
- ✅ Secure online payments
- ✅ Quality assurance

---

## For Providers

- Continuous job opportunities
- Increased online visibility
- Professional portfolio showcase
- Fair bidding system
- Automated payment processing

---

## For Administrators

- Operational dashboards
- Audit logging
- User management
- Financial control
- Reporting tools

---

# 🔄 Core Business Flow

## 1️⃣ Registration & Authentication

- Unified Users table
- Extended Provider Profile
- Support for Individual and Company providers

---

## 2️⃣ Provider Verification (KYC)

1. Provider uploads required documents.
2. Admin reviews documents.
3. Provider status becomes:

- Pending
- Verified
- Rejected

---

## 3️⃣ Job Creation

The customer:

- Writes a job description
- Defines an estimated budget
- Uploads images
- Chooses a service category

The platform then publishes the job to matching providers.

---

## 4️⃣ Provider Bidding

Eligible providers submit:

- Price
- Estimated duration
- Additional notes

---

## 5️⃣ Provider Selection

The customer compares bids and accepts one offer.

The accepted bid becomes an Order.

---

## 6️⃣ Order Lifecycle

```text
Created
   ↓
Pending
   ↓
In Progress
   ↓
Completed
   ↓
Payment Processed
```

Disputed orders can be reviewed by administrators.

---

## 7️⃣ Payment Processing

- Payments are linked to Orders.
- Commission is automatically calculated.
- Provider earnings are computed automatically.
- Invoices are generated.
- Admins monitor all transactions.

---

## 8️⃣ Reviews & Ratings

Customers can submit reviews only after order completion.

Reviews are displayed publicly on provider profiles.

---

## 9️⃣ Chat System

Each Job has its own conversation.

Customers and providers communicate directly throughout the project.

---

# 🧩 Core Modules

| Module | Components |
|---------|------------|
| User Management | Users, Roles, Profiles, Provider Types |
| Services | Categories, Services, Payment Methods |
| Jobs | Jobs, Job Images, Bids |
| Orders & Payments | Orders, Status History, Payments, Commission |
| Communication | Conversations, Messages, Notifications |
| Provider | Portfolio, Documents, Availability, Verification |
| Administration | Audit Logs, Reports, Manual Controls |

---

# 📌 Business Rules

## Users

- Email must be unique.
- Phone number must be unique.
- Providers appear in search only after verification.

---

## Jobs

- Jobs with active bids or orders cannot be permanently deleted.
- Jobs are visible only to matching service categories.

---

## Bids

- One bid per provider for each job.
- Accepted bids cannot be modified.

---

## Orders

- Only verified providers may execute orders.
- Every status change is recorded.
- Orders cannot be closed before payment.

---

## Payments

- Commission is calculated automatically.
- Payments are linked only to Orders.

---

## Reviews

- One review per completed order.
- Reviews require order completion.

---

## Chat

- Every message must include text, an attachment, or both.
- Unread messages are tracked.

---

## Administration

- Sensitive operations are logged.
- Orders and payments cannot be deleted.

---

# 💰 Monetization Model

| Revenue Stream | Description |
|---------------|-------------|
| Commission | Percentage deducted from each completed transaction |
| Provider Boosting | Paid promotion to increase provider visibility |
| Verification Fees | Optional KYC verification fee |
| Subscription Plans | Basic, Pro, and Enterprise plans with premium features |

---

# 📊 Key Performance Indicators (KPIs)

- Order Completion Rate
- Average Provider Rating
- Bid Acceptance Rate
- Average Response Time
- Job-to-Order Conversion Rate
- Monthly Payment Volume
- Customer Complaint Rate

---

# ⚠️ Project Risks

- Slow adoption by customers and providers during the launch phase.
- Delays in payment gateway integration or transaction processing.
- High job cancellation rates affecting customer satisfaction.
- Platform performance issues as user traffic increases.
- Security and data protection risks.
- Downtime or failures in third-party services (payment, email, notifications).
---

# 🚀 BayTack Mission

> **Making home maintenance easier by connecting customers with trusted professionals through transparency, technology, and secure digital services.**
