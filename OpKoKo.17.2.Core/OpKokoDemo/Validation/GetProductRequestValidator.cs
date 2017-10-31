using OpKokoDemo.Models;
using OpKokoDemo.Requests;

namespace OpKokoDemo.Validation
{
    public class GetProductRequestValidator : RequestValidator<GetProductRequest>
    {
        public override Language Language => Language.SE;
        public override bool ValidFor(Language language)
        {
            return language == Language;
        }

        public override void Validate(GetProductRequest request)
        {
            if (request.Pattern != null && request.Pattern.Contains("åäö"))
            {
                AddError(nameof(request.Pattern), "Search pattern may not contain åäö");
            }
        }
    }
}
