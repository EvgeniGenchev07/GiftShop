using BusinessLayer;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLayer
{
    [TestFixture]
    public class FeedbackContextTest
    {
        static FeedbackContext feedbackContext;
        static FeedbackContextTest()
        {
            feedbackContext = new FeedbackContext(TestManager.dbContext);
        }

        [Test]
        public async Task CreateFeedback()
        {
            User user = new User("ivanivanov1@gmail.com","Ivan Ivanov");
            Feedback feedback = new Feedback(user, 4, "Very good service!");

            int countBefore = TestManager.dbContext.Feedbacks.Count();

            await feedbackContext.Create(feedback);

            int countAfter = TestManager.dbContext.Feedbacks.Count();
            Feedback last = TestManager.dbContext.Feedbacks.Last();

            Assert.That(countAfter == countBefore + 1 && last.Review == "Very good service!", "Feedback not added correctly.");
        }

        [Test]
        public async Task ReadFeedback()
        {
            User user = new User("ivanivanov1@gmail.com","Ivan Ivanov");
            Feedback feedback = new Feedback(user, 5, "Amazing!");

            await feedbackContext.Create(feedback);
            Feedback readFeedback = await feedbackContext.Read(feedback.Id);

            Assert.That(readFeedback.Review == "Amazing!", "Read() did not return correct Feedback.");
        }

        [Test]
        public async Task UpdateFeedback()
        {
            User user = new User("ivanivanov1@gmail.com","Ivan Ivanov");
            Feedback feedback = new Feedback(user, 3, "Average experience.");

            await feedbackContext.Create(feedback);

            feedback.Review = "Improved service!";
            feedback.Rating = 4;

            await feedbackContext.Update(feedback);
            Feedback updatedFeedback = await feedbackContext.Read(feedback.Id);

            Assert.That(updatedFeedback.Review == "Improved service!" && updatedFeedback.Rating == 4, "Feedback not updated correctly.");
        }

        [Test]
        public async Task DeleteFeedback()
        {
            User user = new User("ivanivanov1@gmail.com","Ivan Ivanov");
            Feedback feedback = new Feedback(user, 2, "Needs improvement");

            await feedbackContext.Create(feedback);

            List<Feedback> allBefore = await feedbackContext.ReadAll();
            int countBefore = allBefore.Count;

            await feedbackContext.Delete(feedback.Id);

            List<Feedback> allAfter = await feedbackContext.ReadAll();
            int countAfter = allAfter.Count;

            Assert.That(countAfter == countBefore - 1, "Feedback was not deleted correctly.");
        }

        [Test]
        public async Task ReadAllFeedbacks()
        {
            List<Feedback> feedbacksFromDb = await feedbackContext.ReadAll();
            int countFromDb = TestManager.dbContext.Feedbacks.Count();

            Assert.That(feedbacksFromDb.Count == countFromDb, "ReadAll() does not return correct number of feedbacks.");
        }
    }
}
