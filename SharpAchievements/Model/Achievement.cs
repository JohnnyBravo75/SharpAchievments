﻿namespace SharpAchievements.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Achievement
    {
        private decimal completedScore = -1;

        public Achievement(string name = "", List<Rank> ranks = null, string group = "", decimal completedScore = 0)
        {
            this.Name = name;
            if (ranks != null)
            {
                this.Ranks = ranks;
            }
            this.Group = group;
            this.CompletedScore = completedScore;
        }

        public string Group { get; set; } = "Common";

        public string Name { get; set; } = "";

        public bool AutoEarnRankFromScore { get; set; } = false;

        public bool AutoStart { get; set; } = true;

        public bool AutoEndWhenCompleted { get; set; } = true;

        public List<Rank> Ranks { get; set; } = new List<Rank>();

        public List<Badge> Badges { get; set; } = new List<Badge>();

        public decimal CompletedScore
        {
            get
            {
                if (this.completedScore > 0)
                {
                    return this.completedScore;
                }

                if (this.Ranks != null)
                {
                    var lastRank = this.Ranks.LastOrDefault();
                    if (lastRank != null)
                    {
                        return lastRank.Score;
                    }
                }

                return this.completedScore;
            }
            set
            {
                this.completedScore = value;
            }
        }

        public string DependsOnAchievementName { get; set; } = "";

        public TimeSpan? TimeLimit { get; set; }

        public override string ToString()
        {
            return $"{this.Group}: {this.Name}";
        }

        public bool IsCompleted(ScoreData scoreData)
        {
            if (scoreData.IsCompleted)
            {
                return true;
            }

            return scoreData.Score >= this.CompletedScore;  
        }

        public decimal GetPercentageCompleted(ScoreData scoreData)
        {
            if (this.CompletedScore <= 0)
            {
                return 0;   
            }

            return (decimal)scoreData.Score * 100 / this.CompletedScore;
        }

        //public decimal GetPercentageFromRank(string rankName)
        //{
        //    var currentPos = this.GetRankPosition(rankName);
        //    if (currentPos < 0)
        //    {
        //        return 0;
        //    }
        //    var maxPos = this.Ranks.Count();
        //    if (maxPos == 0)
        //    {
        //        return 0;
        //    }

        //    return (decimal)currentPos * 100 / maxPos;
        //}

        public int GetRankPosition(string rankName)
        {
            if (string.IsNullOrEmpty(rankName))
            {
                return 0;
            }

            var rank = this.GetRank(rankName);
            if (rank == null)
            {
                return 0;
            }

            return this.Ranks.IndexOf(rank) + 1;
        }

        public bool IsLastRank(string rankName)
        {
            if (string.IsNullOrEmpty(rankName))
            {
                return false;
            }
            return this.Ranks.LastOrDefault()?.Name == rankName;
        }

        public Rank GetRank(string rankName)
        {
            if (string.IsNullOrEmpty(rankName))
            {
                return null;
            }
            return this.Ranks.FirstOrDefault(x => x.Name == rankName);
        }

        public bool IsValidRank(string rankName)
        {
            return this.GetRank(rankName) != null;
        }


        public Badge GetBadge(string badgeName)
        {
            if (string.IsNullOrEmpty(badgeName))
            {
                return null;
            }
            return this.Badges.FirstOrDefault(x => x.Name == badgeName);
        }

        public bool IsValidBadge(string badgeName)
        {
            return this.GetBadge(badgeName) != null;
        }
    }
}
