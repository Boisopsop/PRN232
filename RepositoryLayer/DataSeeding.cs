using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entities;

namespace RepositoryLayer
{
    public static class DataSeeding
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {
            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    CategoryId = 1,
                    CategoryName = "Academic news",
                    CategoryDesciption = "This category can include articles about research findings, faculty appointments and promotions, and other academic-related announcements.",
                    ParentCategoryId = 1,
                    IsActive = true
                },
                new Category
                {
                    CategoryId = 2,
                    CategoryName = "Student Affairs",
                    CategoryDesciption = "This category can include articles about student activities, events, and initiatives, such as student clubs, organizations and sports.",
                    ParentCategoryId = 2,
                    IsActive = true
                },
                new Category
                {
                    CategoryId = 3,
                    CategoryName = "Campus Safety",
                    CategoryDesciption = "This category can include articles about incidents and safety measures implemented on campus to ensure the safety of students and faculty.",
                    ParentCategoryId = 3,
                    IsActive = true
                },
                new Category
                {
                    CategoryId = 4,
                    CategoryName = "Alumni News",
                    CategoryDesciption = "This category can include articles about the achievements and accomplishments of former students and alumni, such as graduations, job promotions and career successes.",
                    ParentCategoryId = 4,
                    IsActive = true
                },
                new Category
                {
                    CategoryId = 5,
                    CategoryName = "Capstone Project News",
                    CategoryDesciption = "This category is typically a comprehensive and detailed report created as part of an academic or professional capstone project. ",
                    ParentCategoryId = 5,
                    IsActive = false
                }
            );

            // Seed SystemAccounts
            modelBuilder.Entity<SystemAccount>().HasData(
                new SystemAccount
                {
                    AccountId = 1,
                    AccountName = "Emma William",
                    AccountEmail = "EmmaWilliam@FUNewsManagement.org",
                    AccountRole = 2,
                    AccountPassword = "@1"
                },
                new SystemAccount
                {
                    AccountId = 2,
                    AccountName = "Olivia James",
                    AccountEmail = "OliviaJames@FUNewsManagement.org",
                    AccountRole = 2,
                    AccountPassword = "@1"
                },
                new SystemAccount
                {
                    AccountId = 3,
                    AccountName = "Isabella David",
                    AccountEmail = "IsabellaDavid@FUNewsManagement.org",
                    AccountRole = 1,
                    AccountPassword = "@1"
                },
                new SystemAccount
                {
                    AccountId = 4,
                    AccountName = "Michael Charlotte",
                    AccountEmail = "MichaelCharlotte@FUNewsManagement.org",
                    AccountRole = 1,
                    AccountPassword = "@1"
                },
                new SystemAccount
                {
                    AccountId = 5,
                    AccountName = "Steve Paris",
                    AccountEmail = "SteveParis@FUNewsManagement.org",
                    AccountRole = 1,
                    AccountPassword = "@1"
                }
            );

            // Seed Tags
            modelBuilder.Entity<Tag>().HasData(
                new Tag
                {
                    TagId = 1,
                    TagName = "Education",
                    Note = "Education Note"
                },
                new Tag
                {
                    TagId = 2,
                    TagName = "Technology",
                    Note = "Technology Note"
                },
                new Tag
                {
                    TagId = 3,
                    TagName = "Research",
                    Note = "Research Note"
                },
                new Tag
                {
                    TagId = 4,
                    TagName = "Innovation",
                    Note = "Innovation Note"
                },
                new Tag
                {
                    TagId = 5,
                    TagName = "Campus Life",
                    Note = "Campus Life Note"
                },
                new Tag
                {
                    TagId = 6,
                    TagName = "Faculty",
                    Note = "Faculty Achievements"
                },
                new Tag
                {
                    TagId = 7,
                    TagName = "Alumni ",
                    Note = "Alumni News"
                },
                new Tag
                {
                    TagId = 8,
                    TagName = "Events",
                    Note = "University Events"
                },
                new Tag
                {
                    TagId = 9,
                    TagName = "Resources",
                    Note = "Campus Resources"
                }
            );

            // Seed NewsArticles
            modelBuilder.Entity<NewsArticle>().HasData(
                new NewsArticle
                {
                    NewsArticleId = "1",
                    NewsTitle = "University FU Celebrates Success of Alumni in Various Fields",
                    Headline = "University FU Celebrates Success of Alumni in Various Fields",
                    CreatedDate = new DateTime(2024, 5, 5),
                    NewsContent = @"University FU recently commemorated the achievements of its esteemed alumni who have excelled in a multitude of fields, showcasing the impact of the institution's education on their professional journeys.

Diverse Success Stories: From successful entrepreneurs to renowned artists, University X's alumni have made significant strides in various industries, reflecting the versatility of the education provided.

Networking Opportunities: The university's strong alumni network played a crucial role in fostering connections and collaborations among graduates, contributing to their success.

Alumni Contributions: Many alumni have also given back to the university through mentorship programs, scholarships, and guest lectures, enriching the current student experience.",
                    NewsSource = "N/A",
                    CategoryId = 4,
                    NewsStatus = true,
                    CreatedById = 1,
                    UpdatedById = 1,
                    ModifiedDate = new DateTime(2024, 5, 5)
                },
                new NewsArticle
                {
                    NewsArticleId = "2",
                    NewsTitle = "Alumni Association Launches Mentorship Program for Recent Graduates",
                    Headline = "Alumni Association Launches Mentorship Program for Recent Graduates",
                    CreatedDate = new DateTime(2024, 5, 5),
                    NewsContent = @"The Alumni Association of University FU recently unveiled a new mentorship program aimed at providing support and guidance to recent graduates as they navigate the transition from academia to the professional world.

The program pairs recent graduates with experienced alumni mentors based on their career interests and goals, ensuring tailored guidance for each mentee.

In addition to one-on-one mentorship, the program offers workshops on resume building, interview preparation, and networking strategies to equip graduates with essential skills for success.

The mentorship program also facilitates networking events where mentees can expand their professional connections and learn from alumni who have excelled in their respective fields.

By fostering a supportive network of alumni mentors, the program aims to empower recent graduates to navigate the challenges of the job market, enhance their professional growth, and build lasting connections within their industries.

The launch of this mentorship program by the Alumni Association of University Y underscores the commitment to nurturing the success of its graduates beyond graduation, creating a strong community of support and guidance for the next generation of professionals.",
                    NewsSource = "Internet",
                    CategoryId = 4,
                    NewsStatus = true,
                    CreatedById = 1,
                    UpdatedById = 1,
                    ModifiedDate = new DateTime(2024, 5, 5)
                },
                new NewsArticle
                {
                    NewsArticleId = "3",
                    NewsTitle = "Academic Department Announces Groundbreaking Initiatives and Program Enhancements",
                    Headline = "Academic Department Announces Groundbreaking Initiatives and Program Enhancements",
                    CreatedDate = new DateTime(2024, 5, 5),
                    NewsContent = @"The Software Engineering Department at FU has unveiled a series of transformative initiatives and program enhancements aimed at enriching the academic experience and fostering innovation in Software Development.

The department has established [specific research centers or facilities] dedicated to advancing knowledge and addressing pressing challenges in Software Development.

Faculty Promotions: Several esteemed faculty members have been promoted to key leadership positions, reflecting their commitment to academic excellence and their vision for shaping the future of Software Development.

The academic programs within the department have undergone enhancements to incorporate the latest developments and equip students with practical skills and knowledge relevant to current industry demands.

These initiatives are poised to position the Software Engineering Department as a hub of innovation and academic rigor, attracting top talent and fostering groundbreaking research and learning experiences.",
                    NewsSource = "N/A",
                    CategoryId = 1,
                    NewsStatus = true,
                    CreatedById = 2,
                    UpdatedById = 2,
                    ModifiedDate = new DateTime(2024, 5, 5)
                },
                new NewsArticle
                {
                    NewsArticleId = "4",
                    NewsTitle = "Renowned Scholar Appointed as Head of AI Department at FU",
                    Headline = "Renowned Scholar Appointed as Head of AI Department at FU",
                    CreatedDate = new DateTime(2024, 5, 5),
                    NewsContent = @"FU proudly announces the appointment of David Nitzevet, a distinguished scholar in Machine Learning, to the prestigious position of Head of AI Department, underscoring the institution's commitment to academic excellence and leadership.

David Nitzevet brings a wealth of experience and expertise to the role, with a notable track record of the development of deep learning algorithms and the application of machine learning in healthcare, finance, and marketing. In accepting the appointment, David Nitzevet expressed a vision to develop new algorithmic models, improve data preprocessing techniques, and enhance the interpretability of machine learning models, align with the university's mission to drive innovation and excellence in Machine Learning.

The appointment is expected to foster collaborations and initiatives that will enrich the academic and research landscape of the university and beyond.

The addition of David Nitzevet to the AI Department faculty elevates the institution's academic standing and promises to inspire students, scholars, and professionals in Machine Learning. The appointment reaffirms the university's dedication to recruiting top-tier talent and nurturing an environment where academic distinction thrives.",
                    NewsSource = "N/A",
                    CategoryId = 1,
                    NewsStatus = true,
                    CreatedById = 2,
                    UpdatedById = 2,
                    ModifiedDate = new DateTime(2024, 5, 5)
                },
                new NewsArticle
                {
                    NewsArticleId = "5",
                    NewsTitle = "New Research Findings Shed Light on STEM",
                    Headline = "New Research Findings Shed Light on STEM",
                    CreatedDate = new DateTime(2024, 5, 5),
                    NewsContent = @"Groundbreaking research conducted by the Research Department of FU has unveiled significant findings in the field of STEM, offering fresh insights that could revolutionize current understanding and practices.

The success of this research is attributed to the collaborative efforts of a multidisciplinary team, showcasing the institution's commitment to fostering cutting-edge research. The newly uncovered knowledge has the potential to influence Math, Engineering, Technology and shape future research endeavors, positioning the Research Department of FU as a leader in the advancement of STEM.

The research findings stand as a testament to the institution's dedication to impactful research and its contribution to the global knowledge base in STEM.",
                    NewsSource = "N/A",
                    CategoryId = 1,
                    NewsStatus = true,
                    CreatedById = 2,
                    UpdatedById = 2,
                    ModifiedDate = new DateTime(2024, 5, 5)
                }
            );

            // Seed NewsTag (many-to-many relationship)
            modelBuilder.Entity("NewsTag").HasData(
                new { NewsArticleId = "1", TagId = 5 },
                new { NewsArticleId = "1", TagId = 7 },
                new { NewsArticleId = "1", TagId = 9 },
                new { NewsArticleId = "2", TagId = 5 },
                new { NewsArticleId = "2", TagId = 7 },
                new { NewsArticleId = "2", TagId = 8 },
                new { NewsArticleId = "2", TagId = 9 },
                new { NewsArticleId = "3", TagId = 1 },
                new { NewsArticleId = "3", TagId = 8 },
                new { NewsArticleId = "3", TagId = 9 },
                new { NewsArticleId = "4", TagId = 1 },
                new { NewsArticleId = "4", TagId = 4 },
                new { NewsArticleId = "4", TagId = 8 },
                new { NewsArticleId = "4", TagId = 9 },
                new { NewsArticleId = "5", TagId = 2 },
                new { NewsArticleId = "5", TagId = 3 },
                new { NewsArticleId = "5", TagId = 4 },
                new { NewsArticleId = "5", TagId = 6 }
            );
        }
    }
}
