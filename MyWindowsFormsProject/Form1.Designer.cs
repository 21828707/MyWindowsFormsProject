namespace MyWindowsFormsProject
{
    partial class Search1Form
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnSearchItem = new System.Windows.Forms.Button();
            this.btnHistory = new System.Windows.Forms.Button();
            this.btnWishlist = new System.Windows.Forms.Button();
            this.btnDiscounted = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("굴림", 19.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox1.Location = new System.Drawing.Point(62, 85);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(1199, 68);
            this.textBox1.TabIndex = 0;
            // 
            // btnSearchItem
            // 
            this.btnSearchItem.Location = new System.Drawing.Point(1259, 85);
            this.btnSearchItem.Name = "btnSearchItem";
            this.btnSearchItem.Size = new System.Drawing.Size(166, 68);
            this.btnSearchItem.TabIndex = 1;
            this.btnSearchItem.Text = "검색";
            this.btnSearchItem.UseVisualStyleBackColor = true;
            this.btnSearchItem.Click += new System.EventHandler(this.btnSearchItem_Click);
            // 
            // btnHistory
            // 
            this.btnHistory.Location = new System.Drawing.Point(62, 718);
            this.btnHistory.Name = "btnHistory";
            this.btnHistory.Size = new System.Drawing.Size(374, 173);
            this.btnHistory.TabIndex = 2;
            this.btnHistory.Text = "검색기록";
            this.btnHistory.UseVisualStyleBackColor = true;
            this.btnHistory.Click += new System.EventHandler(this.btnHistory_Click);
            // 
            // btnWishlist
            // 
            this.btnWishlist.Location = new System.Drawing.Point(567, 718);
            this.btnWishlist.Name = "btnWishlist";
            this.btnWishlist.Size = new System.Drawing.Size(374, 173);
            this.btnWishlist.TabIndex = 3;
            this.btnWishlist.Text = "찜목록";
            this.btnWishlist.UseVisualStyleBackColor = true;
            this.btnWishlist.Click += new System.EventHandler(this.btnWishlist_Click);
            // 
            // btnDiscounted
            // 
            this.btnDiscounted.Location = new System.Drawing.Point(1051, 718);
            this.btnDiscounted.Name = "btnDiscounted";
            this.btnDiscounted.Size = new System.Drawing.Size(374, 173);
            this.btnDiscounted.TabIndex = 4;
            this.btnDiscounted.Text = "오늘의 특가";
            this.btnDiscounted.UseVisualStyleBackColor = true;
            this.btnDiscounted.Click += new System.EventHandler(this.btnDiscounted_Click);
            // 
            // Search1Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1472, 975);
            this.Controls.Add(this.btnDiscounted);
            this.Controls.Add(this.btnWishlist);
            this.Controls.Add(this.btnHistory);
            this.Controls.Add(this.btnSearchItem);
            this.Controls.Add(this.textBox1);
            this.Name = "Search1Form";
            this.Text = "초기 검색 화면";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnSearchItem;
        private System.Windows.Forms.Button btnHistory;
        private System.Windows.Forms.Button btnWishlist;
        private System.Windows.Forms.Button btnDiscounted;
    }
}

