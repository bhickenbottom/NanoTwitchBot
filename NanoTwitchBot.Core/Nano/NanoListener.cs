namespace NanoTwitchBot.Core.Nano
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NanoTwitchBot.Core.Nano.Model;

    public class NanoListener
    {
        #region Constructors

        public NanoListener(NanoClient nanoClient, string account, TimeSpan interval, int count)
        {
            this.Nano = nanoClient;
            this.Account = account;
            this.Interval = interval;
            this.Count = count;

            // Known Hashes
            this.knownHashes = new List<string>();
        }

        #endregion

        #region Fields

        private List<string> knownHashes;

        #endregion

        #region Properties

        public string Account { get; private set; }

        public int Count { get; private set; }

        public TimeSpan Interval { get; private set; }

        public NanoClient Nano { get; private set; }

        #endregion

        #region Methods

        public Task Start(Action<NanoListenerTransaction> transactionCallback, Action<string> errorCallback)
        {
            return Task.Run(
                async () =>
                {
                    while (true)
                    {
                        try
                        {
                            await Task.Delay(this.Interval);
                            PendingRequest pendingRequest = new PendingRequest();
                            pendingRequest.Account = this.Account;
                            pendingRequest.Count = this.Count.ToString();
                            NanoResult<PendingResponse> pendingResult = await this.Nano.SendAsync<PendingResponse>(pendingRequest);
                            if (pendingResult.IsError(out string pendingError))
                            {
                                errorCallback?.Invoke(pendingError);
                            }

                            BlocksInfoRequest blocksInfoRequest = new BlocksInfoRequest();
                            blocksInfoRequest.Hashes = pendingResult.Response.Blocks;
                            NanoResult<BlocksInfoResponse> blocksInfoResult = await this.Nano.SendAsync<BlocksInfoResponse>(blocksInfoRequest);
                            if (blocksInfoResult.IsError(out string blockInfoError))
                            {
                                errorCallback?.Invoke(blockInfoError);
                            }

                            if (blocksInfoResult.Response.Blocks != null)
                            {
                                foreach ((string key, BlockInfo block) in blocksInfoResult.Response.Blocks)
                                {
                                    if (this.knownHashes.Contains(key))
                                    {
                                        continue;
                                    }

                                    if (block.Confirmed == "true")
                                    {
                                        this.knownHashes.Add(key);
                                        transactionCallback?.Invoke(new NanoListenerTransaction(block));
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            errorCallback?.Invoke(ex.ToString());
                        }
                    }
                });
        }

        #endregion
    }
}