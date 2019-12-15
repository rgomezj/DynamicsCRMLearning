namespace Pavliks.WAM.ManagementConsole.Domain
{
    public enum LineItemType
    {
        None = 0,
        CourseFee = 602300000,
        AmCare = 602300002,
        PmCare = 602300001,
        SupervisedLunch = 602300003,
        CollegeCreditFee = 602300004,

    }
    public enum LineItemTypeInSalesOrder
    {
        CourseFee = 602300000,
        AddonsFee = 602300002,
        ForCreditFee = 602300001,
        RegistrationFee = 804410000,
        CourseCredit = 804410001,
    }
}