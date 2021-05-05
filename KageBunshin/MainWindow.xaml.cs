using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;

//忘れずに書いて
using utauPlugin;

namespace KageBunshin
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        // ここに書いた変数とかはMainWindowクラスのどこからでも呼び出せるよゾーン
        //private UtauPlugin utauPlugin;
        // ↓コマンドライン引数ってやつ
        // この配列の0番目にこのソフトの場所が、
        // 1番目にUTAUが出力したプラグイン用データの場所が書いてある
        string[] args = Environment.GetCommandLineArgs();


        public MainWindow() // 起動時に実行
        {
            // たぶんxamlを読んでる
            InitializeComponent();
            
            // utauPluginを起動してデータの場所を教える
           // utauPlugin = new UtauPlugin(args[1]);
            // utauPluginがデータを分析する
            //utauPlugin.Input();

            // Enter押す＝OKボタン押すのと同義 にする
            KeyDown += (sender, e) =>
            {
                if (e.Key != Key.Enter) { return; }
                OK(sender, e);
            };
        }

        // OKボタン押すと実行
        private void OK(object sender, RoutedEventArgs e)
        {
            try
            {
                /*
                // utauPlugin.noteは音符(Note)がずらーっと入ったList
                // Listの中身をひとつずつ取り出して処理するforeach
                // 今処理してるNoteをnoteと呼ぶ
                foreach (Note note in utauPlugin.note)
                {
                    // 休符かどうか確認
                    if (note.GetLyric() == "R" || note.GetLyric() == "r") continue; // このNoteの処理をやめて次のNoteにいく

                    /*処理***/

                    /*
					if (SelModBox.IsChecked.Value)
					{
						int newMod = ((SelectModBias.IsChecked.Value)? note.GetMod()  : (int)ModBias.Value ) + makeRund((int)ModVar.Value);
						note.SetMod(newMod);
					}

					//VO+PU
					if (SelVO_PUBox.IsChecked.Value) 
					{
						double newPu = ((SelectPUBias.IsChecked.Value)? note.GetAtPre() : note.GetPre()+PUBias.Value) + makeRund((int)PUvar.Value);

						double newOv = ((SelectVOBias.IsChecked.Value) ? note.GetAtOve() : note.GetOve()+PUBias.Value) + makeRund((int)PUvar.Value);

						if (newOv >= newPu){
							double temp = newPu;
							newPu = newOv;
							newOv = temp;
						}

						note.SetPre(newPu.ToString());

                        //HopeTodo:エンベロープの最適化
                        /*https://github.com/delta-kimigatame/utauPlugin
                         * Envelope
                            Noteから直接できない操作のみ説明 Note.envelopeがEnvelope型なので，
                        'Note.envelope.GetP()'のような使い方を想定しています．

                            メソッド	説明
                            void SetP(float p,int point)	'point'個目(0スタート)のエンベロープのpを変更します．
                            void SetV(int p,int point)	'point'個目(0スタート)のエンベロープのvを変更します．
                            List GetP()	エンベロープのpを取得
                            List GetV()	エンベロープのvを取得
                         
                    }


                    //Velocity
                    if (SelVeloBox.IsChecked.Value)
					{
						int newVelocity = (SelectVelocityBias.IsChecked.Value ? note.GetVelocity()  : (int)VelocityBias.Value ) + makeRund((int)VelocityVar.Value);
						if (newVelocity < 0) newVelocity = 0;
						else if (200 < newVelocity) newVelocity = 200;
						note.SetVelocity(newVelocity);
					}

                    //string:Flags
                    if (SelFlagsBox.IsChecked.Value)
					{
                        string fixedFlagsSorts = FixedFlagsSorts.Text;
                        string fixedFlagsValue = FixedFlagsSorts.Text;

                        string variableFlagsSorts = VariableFlagsSorts.Text;
                        string variableFlagsBiasValue = SelectFlagsBias.IsChecked.Value ? note.GetFlags() : VariableFlagsVarValue.Text;
                        string variableFlagsVarValue = VariableFlagsVarValue.Text;

                        //変動フラグ種類の数だけ
                        char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
                        string[] variableSplitedFlagsSort = variableFlagsSorts.Split(delimiterChars);
                        string[] variableSplitedBiasValue = variableFlagsBiasValue.Split(delimiterChars);
                        string[] variableSplitedVarValue = variableFlagsVarValue.Split(delimiterChars);
                        string[] variableSplitedNewValue = { };
                        for (int i = 0; i < variableSplitedFlagsSort.Length; i++) {
                            Array.Resize(ref variableSplitedNewValue, variableSplitedNewValue.Length + 1);
                            variableSplitedNewValue[variableSplitedNewValue.Length - 1] =""+ Int32.Parse(variableSplitedBiasValue[i]) + makeRund(Int32.Parse(variableSplitedVarValue[i]));
                        }

                        //"[固定フラグ種類=固定フラグ値]*,[変動フラグ=新しい変動フラグ値]*"の書式で1つの文字列にする
                        string[] fixedSplitedFlagsSort = fixedFlagsSorts.Split(delimiterChars);
                        string[] fixedSplitedValue = fixedFlagsValue.Split(delimiterChars);
                        string newFlags = "";
                        for (int i = 0; i < fixedSplitedFlagsSort.Length; i++) newFlags += fixedSplitedFlagsSort[i] +"="+fixedSplitedValue[i];
                        for (int i = 0; i < variableSplitedFlagsSort.Length; i++) newFlags += variableSplitedFlagsSort[i] + "=" + variableSplitedNewValue[i];
                        //値のセット
                        note.SetFlags(newFlags);

                    }

					//Pitch
					if (SelPitBox.IsChecked.Value)
					{

                        if (PitchModeSelecter.SelectedIndex == 0) {//mode1
                            List<int> newPitches = new List<int>() { };
                            List<int> basePitches = note.GetPitches();
                            foreach (int bp in basePitches)
                            {
                                newPitches.Add((SelectPitchMode1Bias.IsChecked.Value) ? bp : (int)PitchBias.Value + makeRund((int)PitchVar.Value));
                            }
                            note.SetPitches(newPitches);

                            //PBStart=mode1用ピッチ数列の開始位置:float SetPbStart  GetPbStart 0.0f
                            //PBType = mode1用ピッチベンドの種類:string	SetPbType	GetPbType "5"
                        }
                        else if (PitchModeSelecter.SelectedIndex == 1) {//mode2
                            //BS = mode2用ピッチの開始位置  string SetPbs  GetPbs  "-50"
                            //PBW = mode2用ポルタメントの間隔 List<float> SetPbw  GetPbw 空のList
                            //PBY = mode2用ポルタメントの音高 List<float> SetPby  GetPby 空のList
                            //PBM = mode2用ピッチベンドの種類 List<string> SetPbm  GetPbm 空のList これはいじらない

                            //VBR=	ビブラート	Vibrato	InitVibrato	HasVibrato	SetVibrato	GetVibrato	VibratoIsChanged	"65,180,35,20,20,0,0,0"
                            /*
                             *  Length	float	ノート長に対するビブラート長
                                Cycle	float	ビブラートの周期
                                Depth	float	ビブラートの深さ
                                FadeInTime	float	ビブラート長に対するフェードインの割合
                                FadeoutTime	float	ビブラート長に対するフェードアウトの割合
                                Phase	float	ビブラートの初期位相のずれ
                                Height	float	ビブラートの音程オフセット
                             
                        }
                        else//未選択の例外
                        {
                            throw new Exception("PitchModeが選択されませんでした");
                        }
					}
				}

                // UTAU本体に処理したデータを返す
                utauPlugin.Output();
                    */
            }
            catch // try中にエラー吐いたとき、落ちない代わりに↓が実行される
            {
                ErrorOpen("処理に失敗しました"); // この内容は後述
                return; // OKメソッドをやめる（次のCloseは起きない）
            }

            Close(); // ソフトを終了
        }

        // キャンセルボタン押すと実行
        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }


        //エラードロワーを開く
        public void ErrorOpen(string text)
        {
            // xamlに書いたTextBlockにエラーメッセージを設定
            errorText.Text = text;
            // 警告音を鳴らす
            System.Media.SystemSounds.Exclamation.Play();
            // ドロワーを出す
            errorDrawer.IsBottomDrawerOpen = true;
        }

		//特定範囲内でランダム値を返すメソッド
		public int makeRund(int variance)
        {
			int seed = Environment.TickCount;

			Random rnd = new Random(seed*variance);
			return rnd.Next(variance)  -rnd.Next(variance);
        }
    }
                }
