/*
* Copyright (c) 2025 Original Author(s), PhonePe India Pvt. Ltd.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

namespace pg_sdk_dotnet.Common.Models.Request.PaymentModeConstraints;
public class CardPaymentModeConstraint(HashSet<CardType>? cardTypes = null) : PaymentModeConstraint(PgV2InstrumentType.CARD)
{
    public HashSet<CardType>? CardTypes { get; private set; } = cardTypes;

    public static CardPaymentModeConstraintBuilder Builder()
    {
        return new CardPaymentModeConstraintBuilder();
    }

    public class CardPaymentModeConstraintBuilder
    {
        private HashSet<CardType>? _cardTypes;

        public CardPaymentModeConstraintBuilder SetCardTypes(HashSet<CardType>? cardTypes)
        {
            this._cardTypes = cardTypes;
            return this;
        }

        public CardPaymentModeConstraint Build()
        {
            return new CardPaymentModeConstraint(this._cardTypes);
        }
    }
}