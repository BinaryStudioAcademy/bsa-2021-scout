using Domain.Common;

namespace Domain.Entities
{
    public class ReviewToStage : Entity
    {
        public string StageId { get; set; }
        public string ReviewId { get; set; }

        public Stage Stage { get; set; }
        public Review Review { get; set; }
    }
}
