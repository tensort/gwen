using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Police
{
    public enum LOCATION_TYPE
    {
        Force, British_Transport_Police
    }

    public enum OUTCOME_TYPE
    {
        UNKNOWN,
        AWAITING_COURT_RESULT,
        COURT_RESULT_UNAVAILABLE,
        UNABLE_TO_PROCEED,
        LOCAL_RESOLUTION,
        NO_FURTHER_ACTION,
        DEPRIVED_OF_PROPERTY,
        FINED,
        ABSOLUTE_DISCHARGE,
        CAUTIONED,
        DRUGS_POSSESSION_WARNING,
        PENALTY_NOTICE_ISSUED,
        COMMUNITY_PENALTY,
        CONDITIONAL_DISCHARGE,
        SUSPENDED_SENTENCE,
        IMPRISONED,
        OTHER_COURT_DISPOSAL,
        COMPENSATION,
        SENTENCED_IN_ANOTHER_CASE,
        CHARGED,
        NOT_GUILTY,
        SENT_TO_CROWN_COURT,
        UNABLE_TO_PROSECUTE,
        FORMAL_ACTION_NOT_IN_PUBLIC_INTEREST,
        UNDER_INVESTIGATION
    }

    public class Outcome
    {
        public string Description { get; private set; }
        public string Code { get; private set; }

        public Outcome(string description, string code)
        {
            Description = description;
            Code = code;
        }

        public static OUTCOME_TYPE ConvertToOutcomeType(string s)
        {
            switch (s)
            {
                case "awaiting-court-result":
                    return OUTCOME_TYPE.AWAITING_COURT_RESULT;
                case "court-result-unavailable":
                    return OUTCOME_TYPE.COURT_RESULT_UNAVAILABLE;
                case "unable-to-proceed":
                    return OUTCOME_TYPE.UNABLE_TO_PROCEED;
                case "local-resolution":
                    return OUTCOME_TYPE.LOCAL_RESOLUTION;
                case "no-further-action":
                    return OUTCOME_TYPE.NO_FURTHER_ACTION;
                case "deprived-of-property":
                    return OUTCOME_TYPE.DEPRIVED_OF_PROPERTY;
                case "fined":
                    return OUTCOME_TYPE.FINED;
                case "absolute-discharge":
                    return OUTCOME_TYPE.ABSOLUTE_DISCHARGE;
                case "cautioned":
                    return OUTCOME_TYPE.CAUTIONED;
                case "drugs-possession-warning":
                    return OUTCOME_TYPE.DRUGS_POSSESSION_WARNING;
                case "penalty-notice-issued":
                    return OUTCOME_TYPE.PENALTY_NOTICE_ISSUED;
                case "community-penalty":
                    return OUTCOME_TYPE.COMMUNITY_PENALTY;
                case "conditional-discharge":
                    return OUTCOME_TYPE.CONDITIONAL_DISCHARGE;
                case "suspended-sentence":
                    return OUTCOME_TYPE.SUSPENDED_SENTENCE;
                case "imprisoned":
                    return OUTCOME_TYPE.IMPRISONED;
                case "other-court-disposal":
                    return OUTCOME_TYPE.OTHER_COURT_DISPOSAL;
                case "compensation":
                    return OUTCOME_TYPE.COMPENSATION;
                case "sentenced-in-another-case":
                    return OUTCOME_TYPE.SENTENCED_IN_ANOTHER_CASE;
                case "charged":
                    return OUTCOME_TYPE.CHARGED;
                case "not-guilty":
                    return OUTCOME_TYPE.NOT_GUILTY;
                case "sent-to-crown-court":
                    return OUTCOME_TYPE.SENT_TO_CROWN_COURT;
                case "unable-to-prosecute":
                    return OUTCOME_TYPE.UNABLE_TO_PROCEED;
                case "formal-action-not-in-public-interest":
                    return OUTCOME_TYPE.FORMAL_ACTION_NOT_IN_PUBLIC_INTEREST;
                case "under-investigation":
                    return OUTCOME_TYPE.UNDER_INVESTIGATION;
                default:
                    return OUTCOME_TYPE.UNKNOWN;
            }
        }

        public static Outcome ConvertToOutcome(OUTCOME_TYPE k)
        {
            switch (k)
            {
				case OUTCOME_TYPE.AWAITING_COURT_RESULT:
                    return new Outcome("Awaiting court outcome", "awaiting-court-result");
					
				case OUTCOME_TYPE.COURT_RESULT_UNAVAILABLE:
                    return new Outcome("Court result unavailable", "court-result-unavailable");
					
				case OUTCOME_TYPE.UNABLE_TO_PROCEED:
                    return new Outcome("Court case unable to proceed", "unable-to-proceed");
					
				case OUTCOME_TYPE.LOCAL_RESOLUTION:
                    return new Outcome("Local resolution", "local-resolution");
					
				case OUTCOME_TYPE.NO_FURTHER_ACTION:
                    return new Outcome("Investigation complete; no suspect identified", "no-further-action");
					
				case OUTCOME_TYPE.DEPRIVED_OF_PROPERTY:
                    return new Outcome("Offender deprived of property", "deprived-of-property");
					
				case OUTCOME_TYPE.FINED:
                    return new Outcome("Offender fined", "fined");
					
				case OUTCOME_TYPE.ABSOLUTE_DISCHARGE:
                    return new Outcome("Offender given absolute discharge", "absolute-discharge");
					
				case OUTCOME_TYPE.CAUTIONED:
                    return new Outcome("Offender given a caution", "cautioned");
					
				case OUTCOME_TYPE.DRUGS_POSSESSION_WARNING:
                    return new Outcome("Offender given a drugs possession warning", "drugs-possession-warning");
					
				case OUTCOME_TYPE.PENALTY_NOTICE_ISSUED:
                    return new Outcome("Offender given a penalty notice", "penalty-notice-issued");
					
				case OUTCOME_TYPE.COMMUNITY_PENALTY:
                    return new Outcome("Offender given community sentence", "community-penalty");
					
				case OUTCOME_TYPE.CONDITIONAL_DISCHARGE:
                    return new Outcome("Offender given conditional discharge", "conditional-discharge");
					
				case OUTCOME_TYPE.SUSPENDED_SENTENCE:
                    return new Outcome("Offender given suspended prison sentence", "suspended-sentence");
					
				case OUTCOME_TYPE.IMPRISONED:
                    return new Outcome("Offender sent to prison", "imprisoned");
					
				case OUTCOME_TYPE.OTHER_COURT_DISPOSAL:
                    return new Outcome("Offender otherwise dealt with", "other-court-disposal");
					
				case OUTCOME_TYPE.COMPENSATION:
                    return new Outcome("Offender ordered to pay compensation", "compensation");
					
				case OUTCOME_TYPE.SENTENCED_IN_ANOTHER_CASE:
                    return new Outcome("Suspect charged as part of another case", "sentenced-in-another-case");
					
				case OUTCOME_TYPE.CHARGED:
                    return new Outcome("Suspect charged", "charged");
					
				case OUTCOME_TYPE.NOT_GUILTY:
                    return new Outcome("Defendant found not guilty", "not-guilty");
					
				case OUTCOME_TYPE.SENT_TO_CROWN_COURT:
                    return new Outcome("Defendant sent to Crown Court", "sent-to-crown-court");
					
				case OUTCOME_TYPE.UNABLE_TO_PROSECUTE:
                    return new Outcome("Unable to prosecute suspect", "unable-to-prosecute");
					
				case OUTCOME_TYPE.FORMAL_ACTION_NOT_IN_PUBLIC_INTEREST:
                    return new Outcome("Formal action is not in the public interest", "formal-action-not-in-public-interest");
					
				case OUTCOME_TYPE.UNDER_INVESTIGATION:
                    return new Outcome("Under investigation", "under-investigation");
				
                default:
                    return new Outcome("Outcome unknown", null);
            }
        }
    }
}
