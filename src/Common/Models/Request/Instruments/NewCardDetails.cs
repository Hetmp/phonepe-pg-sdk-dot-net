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

namespace pg_sdk_dotnet.Common.Models.Request.Instruments;
public class NewCardDetails
{
    public string EncryptedCardNumber { get; private set; }
    
    public long EncryptionKeyId { get; private set; }

    public string EncryptedCvv { get; private set; }

    public Expiry Expiry { get; private set; }

    public string? CardHolderName { get; private set; }

    public BillingAddress? BillingAddress { get; set; }

    private NewCardDetails(string encryptedCardNumber, string encryptedCvv, long encryptionKeyId, Expiry expiry, string? cardHolderName, BillingAddress billingAddress)
    {
        EncryptedCardNumber = encryptedCardNumber;
        EncryptedCvv = encryptedCvv;
        EncryptionKeyId = encryptionKeyId;
        Expiry = expiry;
        CardHolderName = cardHolderName;
        BillingAddress = billingAddress;
    }

    public static NewCardDetailsBuilder Builder() => new();

    public class NewCardDetailsBuilder
    {
        private string _encryptedCardNumber = null!;
        private string _encryptedCvv = null!;
        private long _encryptionKeyId;
        private Expiry _expiry = null!;
        private string? _cardHolderName;
        private BillingAddress? _billingAddress;

        public NewCardDetailsBuilder SetEncryptedCardNumber(string encryptedCardNumber)
        {
            this._encryptedCardNumber = encryptedCardNumber;
            return this;
        }

        public NewCardDetailsBuilder SetEncryptedCvv(string encryptedCvv)
        {
            this._encryptedCvv = encryptedCvv;
            return this;
        }

        public NewCardDetailsBuilder SetEncryptionKeyId(long encryptionKeyId)
        {
            this._encryptionKeyId = encryptionKeyId;
            return this;
        }

        public NewCardDetailsBuilder SetExpiry(Expiry expiry)
        {
            this._expiry = expiry;
            return this;
        }

        public NewCardDetailsBuilder SetCardHolderName(string? cardHolderName)
        {
            this._cardHolderName = cardHolderName;
            return this;
        }

        public NewCardDetailsBuilder SetBillingAddress(BillingAddress? billingAddress)
        {
            this._billingAddress = billingAddress;
            return this;
        }

        public NewCardDetails Build()
        {
            return new NewCardDetails(this._encryptedCardNumber, this._encryptedCvv, this._encryptionKeyId, this._expiry, this._cardHolderName, this._billingAddress!);
        }
    }
}