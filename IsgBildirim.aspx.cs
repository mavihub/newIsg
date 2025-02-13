﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace serkanISG
{
    public partial class IsgBildirim : System.Web.UI.Page
    {

        serkanISGEntities1 db = new serkanISGEntities1();

       
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Serkan";

            if (!IsPostBack && !IsCallback)
            {

                try
                {
                   
                    bind();
                  
                }
                catch (Exception)
                {

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "myFunction('Hay Aksi!  Beklenmedik bir hata oluştu :(','fail');", true);
                }




            }
        }

        private void bind()
        {

            if (ddlAktiflik.SelectedValue == "Pasif")
            {
                grdBILDIRIM.DataSource = db.BILDIRIMLVL1.Where(i => i.DURUM == "Pasif").ToList();
                grdBILDIRIM.DataBind();

            }
            else
            {

                grdBILDIRIM.DataSource = db.BILDIRIMLVL1.Where(i => i.DURUM == "Aktif").ToList();
                grdBILDIRIM.DataBind();



            }


          



            ddlVardiya.DataSource = db.VARDIYA.Select(i => i.AD_VARDIYA).ToList();
            ddlBildirimYapan.DataSource = db.PERSONEL.Select(i => i.PERSONEL_AD.ToUpper() + " " + i.PERSONEL_SOYAD.ToUpper()).ToList();
            ddlBirim.DataSource = db.BIRIM.Select(i => i.AD_BIRIM).ToList();
            ddlKategori.DataSource = db.KATEGORI.Where(i => i.DURUM == "Aktif").Select(i => i.AD_KATEGORI).ToList();
            ddlLokasyon.DataSource = db.LOKASYONN.Select(i => i.AD_LOKASYON).ToList();
            ddlMudahilPerson.DataSource = db.PERSONEL.Select(i => i.PERSONEL_AD.ToUpper() + " " + i.PERSONEL_SOYAD.ToUpper()).ToList();
            ddlSorumluPersonel.DataSource = db.PERSONEL.Select(i => i.PERSONEL_AD.ToUpper() + " " + i.PERSONEL_SOYAD.ToUpper()).ToList();

            //SAdsad
            ddlVardiya_Edit.DataSource = db.VARDIYA.Select(i => i.AD_VARDIYA).ToList();
            ddlBildirimYapan_Edit.DataSource = db.PERSONEL.Select(i => i.PERSONEL_AD.ToUpper() + " " + i.PERSONEL_SOYAD.ToUpper()).ToList();
            ddlBirim_Edit.DataSource = db.BIRIM.Select(i => i.AD_BIRIM).ToList();
            ddlKategori_Edit.DataSource = db.KATEGORI.Where(i => i.DURUM == "Aktif").Select(i => i.AD_KATEGORI).ToList();
            ddl_Lokasyon_Edit.DataSource = db.LOKASYONN.Select(i => i.AD_LOKASYON).ToList();
            ddlMudahilPersonel_edit.DataSource = db.PERSONEL.Select(i => i.PERSONEL_AD.ToUpper() + " " + i.PERSONEL_SOYAD.ToUpper()).ToList();

            ddlLokasyon.AppendDataBoundItems = true;
            ddlLokasyon.Items.Insert(0, new ListItem("Seçiniz", ""));

            ddlVardiya.DataBind();
            ddlBildirimYapan.DataBind();
            ddlBirim.DataBind();
            ddlKategori.DataBind();
            ddlLokasyon.DataBind();
            ddlMudahilPerson.DataBind();
            ddlSorumluPersonel.DataBind();


            //

            ddlVardiya_Edit.DataBind();
            ddlBildirimYapan_Edit.DataBind();
            ddlBirim_Edit.DataBind();
            ddlKategori_Edit.DataBind();
            ddl_Lokasyon_Edit.DataBind();
            ddlMudahilPersonel_edit.DataBind();



            txtTarih.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtSaat.Text = DateTime.Now.ToString("HH:mm");
            txtTerminTarih.Text = DateTime.Now.ToString("yyyy-MM-dd");

        }




        protected void btnEKLE_Click(object sender, EventArgs e)
        {

            try
            {
                BILDIRIMLVL1 yeniBildirim = new BILDIRIMLVL1();

                string tarih = txtTarih.Text;
                string saat = txtSaat.Text;
                string Date = tarih + " " + saat;
                yeniBildirim.TARIHSAAT = Date;
                yeniBildirim.VARDIYA = ddlVardiya.Text;
                yeniBildirim.PERSONEL_AD = ddlBildirimYapan.Text;
                yeniBildirim.BIRIM = ddlBirim.Text;
                yeniBildirim.KATEGORI = ddlKategori.Text;
                yeniBildirim.LOKASYON = ddlLokasyon.Text;
                yeniBildirim.ONLEMBOOL = ddlOnlemBool.Text;
                yeniBildirim.ACIKLAMA = txtBildirimMetin.Text;
                yeniBildirim.AKSIYON = txtAksiyon.Text;
                yeniBildirim.GORSEL = imgBildirim.FileName.ToString();
                yeniBildirim.MUDAHIL_PERSONEL = ddlMudahilPerson.Text;
                yeniBildirim.DURUM = "Aktif";
                yeniBildirim.BILDIRIM_DURUM = "Onay Bekliyor";

                uygunsuzlukEkle(kontrol);


                imgBildirim.PostedFile.SaveAs(Server.MapPath("~/upload/") + imgBildirim.FileName.ToString());

                db.BILDIRIMLVL1.Add(yeniBildirim);
                if (db.SaveChanges() > 0)
                {

                    MODUL_MAILAYAR mailOnay = new MODUL_MAILAYAR();
                    mailOnay = db.MODUL_MAILAYAR.FirstOrDefault(i => i.ID_MODUL == 1);
                    MailSend ms = new MailSend();
                    PERSONEL per = new PERSONEL();
                    string fullad = ddlMudahilPerson.Text;
                    string[] parce = fullad.Split(' ');
                    string ad = parce[0];

                    per = db.PERSONEL.FirstOrDefault(i => i.PERSONEL_AD == ad);
                    string MudahilMail = per.EMAIL;


                    string c = "&lt;table width=&quot;100%&quot; border=&quot;0&quot; cellspacing=&quot;0&quot;&gt; &lt;tbody&gt; &lt;tr&gt; &lt;td align=&quot;center&quot; valign=&quot;top&quot; style=&quot;background-color: #f0f0f0; padding: 20px&quot;&gt; &lt;table width=&quot;100%&quot; border=&quot;0&quot; cellspacing=&quot;0&quot; style=&quot;box-sizing: border-box;&quot;&gt; &lt;tbody&gt; &lt;tr&gt; &lt;td style=&quot;background-color: #4791d2; border-bottom: 2px solid #367fbe; height: 6px;&quot;&gt;&lt;/td&gt; &lt;/tr&gt; &lt;tr&gt; &lt;td style=&quot;background-color: #fff; text-align: left; padding: 20px;&quot;&gt; &lt;p style=&quot;font-family: Tahoma; font-size: 12px&quot;&gt; Sayın "+ddlMudahilPerson.Text+",&lt;br /&gt; Seviye 1 İSG Bildiriminde M&#252;dahil Personel Olarak Atandınız. &lt;/p&gt; &lt;table style=&quot;width: 100%; border-width: 1px; border-style: solid; border-collapse: collapse; font-size: 10pt; font-family: Tahoma;&quot; bordercolor=&quot;black&quot;&gt; &lt;tbody&gt; &lt;tr style=&quot;width: 100%; border: 1px solid black; border-collapse: collapse; background-color: #FFFFFF;&quot;&gt; &lt;td style=&quot;width: 20%; border: 1px solid black; border-collapse: collapse; text-align: left; font-weight: bold;&quot;&gt;Tarih ve Saat&lt;/td&gt; &lt;td style=&quot;width: 80%; border: 1px solid black; border-collapse: collapse; text-align: left;&quot;&gt;" + Date + "&lt;/td&gt; &lt;/tr&gt; &lt;tr style=&quot;width: 100%; border: 1px solid black; border-collapse: collapse; background-color: #F7FBFF;&quot;&gt; &lt;td style=&quot;width: 20%; border: 1px solid black; border-collapse: collapse; text-align: left; font-weight: bold;&quot;&gt;Lokasyon&lt;/td&gt; &lt;td style=&quot;width: 80%; border: 1px solid black; border-collapse: collapse; text-align: left;&quot;&gt;"+ddlLokasyon.Text+"&lt;/td&gt; &lt;/tr&gt; &lt;tr style=&quot;width: 100%; border: 1px solid black; border-collapse: collapse; background-color: #FFFFFF;&quot;&gt; &lt;td style=&quot;width: 20%; border: 1px solid black; border-collapse: collapse; text-align: left; font-weight: bold;&quot;&gt;Birim&lt;/td&gt; &lt;td style=&quot;width: 80%; border: 1px solid black; border-collapse: collapse; text-align: left;&quot;&gt;"+ddlBirim.Text+"&lt;/td&gt; &lt;/tr&gt; &lt;tr style=&quot;width: 100%; border: 1px solid black; border-collapse: collapse; background-color: #F7FBFF;&quot;&gt; &lt;td style=&quot;width: 20%; border: 1px solid black; border-collapse: collapse; text-align: left; font-weight: bold;&quot;&gt;Kategori&lt;/td&gt; &lt;td style=&quot;width: 80%; border: 1px solid black; border-collapse: collapse; text-align: left;&quot;&gt;"+ddlKategori.Text+"&lt;/td&gt; &lt;/tr&gt; &lt;tr style=&quot;width: 100%; border: 1px solid black; border-collapse: collapse; background-color: #FFFFFF;&quot;&gt; &lt;td style=&quot;width: 20%; border: 1px solid black; border-collapse: collapse; text-align: left; font-weight: bold;&quot;&gt;Vardiya&lt;/td&gt; &lt;td style=&quot;width: 80%; border: 1px solid black; border-collapse: collapse; text-align: left;&quot;&gt;"+ddlVardiya.Text+"&lt;/td&gt; &lt;/tr&gt; &lt;tr style=&quot;width: 100%; border: 1px solid black; border-collapse: collapse; background-color: #FFFFFF;&quot;&gt; &lt;td style=&quot;width: 20%; border: 1px solid black; border-collapse: collapse; text-align: left; font-weight: bold;&quot;&gt;Bildiren Personel&lt;/td&gt; &lt;td style=&quot;width: 80%; border: 1px solid black; border-collapse: collapse; text-align: left;&quot;&gt;"+ddlBildirimYapan.Text+"&lt;/td&gt; &lt;/tr&gt; &lt;tr style=&quot;width: 100%; border: 1px solid black; border-collapse: collapse; background-color: #FFFFFF;&quot;&gt; &lt;td style=&quot;width: 20%; border: 1px solid black; border-collapse: collapse; text-align: left; font-weight: bold;&quot;&gt;Bildirim Metni&lt;/td&gt; &lt;td style=&quot;width: 80%; border: 1px solid black; border-collapse: collapse; text-align: left;&quot;&gt;"+txtBildirimMetin.Text+"&lt;/td&gt; &lt;/tr&gt; &lt;/tbody&gt; &lt;/table&gt; &lt;/td&gt; &lt;/tr&gt; &lt;tr&gt; &lt;td style=&quot;background-color: #4791d2; border-top: 2px solid #367fbe; height: 6px;&quot;&gt;&lt;/td&gt; &lt;/tr&gt; &lt;/tbody&gt; &lt;/table&gt; &lt;/td&gt; &lt;/tr&gt; &lt;/tbody&gt; &lt;/table&gt;";


                   
                    ms.MailGonder(mailOnay.YENI_KAYIT,MudahilMail, "Seviye-1 İSG Bildirimi", HttpUtility.HtmlDecode(c).ToString());





                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "myFunction('Bildirim Eklendi','succsess');", true);
                    bind();

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "myFunction(Hay Aksi :('Bildirim Kaydı Oluşturulamadı'fail');", true);
                }




            }
            catch (Exception)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "myFunction(Hay Aksi :(','fail');", true);
            }


        }


        public void uygunsuzlukEkle(CheckBox kontrol)
        {
            if (kontrol.Checked == true)
            {
                UYGUNSUZLUK yeniUygsunsuzluk = new UYGUNSUZLUK();
                yeniUygsunsuzluk.BIRIM = ddlBirim.Text;
                yeniUygsunsuzluk.AKTIFLIK = "1";
                yeniUygsunsuzluk.ONERI_AKSIYON = txtAksiyon.Text;
                yeniUygsunsuzluk.TERMIN_TARIH = Convert.ToDateTime(txtTerminTarih.Text);
                yeniUygsunsuzluk.TESPIT_EDEN_AD = ddlBildirimYapan.Text;
                yeniUygsunsuzluk.TESPIT_TARIH = Convert.ToDateTime(txtTarih.Text);
                yeniUygsunsuzluk.TUR = "Minör";
                yeniUygsunsuzluk.UYGUNSUZ_DURUM = txtUygunsuzDurum.Text;
                yeniUygsunsuzluk.SORUMLU_AD = ddlSorumluPersonel.Text;
                yeniUygsunsuzluk.AKTIFLIK = "Aktif";
                db.UYGUNSUZLUK.Add(yeniUygsunsuzluk);


            }
            else
            {
                return;
            }
        }

        protected void btn_sil_Click(object sender, EventArgs e)
        {
            try
            {


                MODUL_MAILAYAR mailOnay = new MODUL_MAILAYAR();
                mailOnay = db.MODUL_MAILAYAR.FirstOrDefault(i => i.ID_MODUL == 1);
                MailSend ms = new MailSend();
                BILDIRIMLVL1 deleteBILDIRIM = new BILDIRIMLVL1();
                LinkButton linkbutton = (LinkButton)sender;                 // get the link button which trigger the event
                GridViewRow row = (GridViewRow)linkbutton.NamingContainer; // get the GridViewRow that contains the linkbutton
                                                                           // get the first cell value of the row
                                                                           // if you want to get controls in templatefield , just use row.FindControl
                int deleteID = Convert.ToInt32(linkbutton.CommandArgument);

                deleteBILDIRIM = db.BILDIRIMLVL1.FirstOrDefault(i => i.ID_BILDIRIM == deleteID);
                deleteBILDIRIM.DURUM = "Pasif";
                deleteBILDIRIM.BILDIRIM_DURUM = "Silinmiş";

                PERSONEL per = new PERSONEL();
                string fullad = deleteBILDIRIM.MUDAHIL_PERSONEL;
                string[] parce = fullad.Split(' ');
                string ad = parce[0];

                per = db.PERSONEL.FirstOrDefault(i => i.PERSONEL_AD.ToUpper() == ad.ToUpper());
                string MudahilMail = per.EMAIL;




                if (db.SaveChanges() > 0)
                {


                    string c = "&lt;table width=&quot;100%&quot; border=&quot;0&quot; cellspacing=&quot;0&quot;&gt; &lt;tbody&gt; &lt;tr&gt; &lt;td align=&quot;center&quot; valign=&quot;top&quot; style=&quot;background-color: #f0f0f0; padding: 20px&quot;&gt; &lt;table width=&quot;100%&quot; border=&quot;0&quot; cellspacing=&quot;0&quot; style=&quot;box-sizing: border-box;&quot;&gt; &lt;tbody&gt; &lt;tr&gt; &lt;td style=&quot;background-color: #4791d2; border-bottom: 2px solid #367fbe; height: 6px;&quot;&gt;&lt;/td&gt; &lt;/tr&gt; &lt;tr&gt; &lt;td style=&quot;background-color: #fff; text-align: left; padding: 20px;&quot;&gt; &lt;p style=&quot;font-family: Tahoma; font-size: 12px&quot;&gt; Sayın " + deleteBILDIRIM.MUDAHIL_PERSONEL + " &quot;,&lt;br /&gt; Seviye 1 İSG Bildiriminde M&#252;dahil Personel Olarak Atandığınız Bildirim Silinmiştir. &lt;br/&gt; &lt;br/&gt;Silinen bildirimin detaylarını aşağıdaki tablodan inceleyebilirsiniz. &lt;/p&gt; &lt;table style=&quot;width: 100%; border-width: 1px; border-style: solid; border-collapse: collapse; font-size: 10pt; font-family: Tahoma;&quot; bordercolor=&quot;black&quot;&gt; &lt;tbody&gt; &lt;tr style=&quot;width: 100%; border: 1px solid black; border-collapse: collapse; background-color: #FFFFFF;&quot;&gt; &lt;td style=&quot;width: 20%; border: 1px solid black; border-collapse: collapse; text-align: left; font-weight: bold;&quot;&gt;Tarih ve Saat&lt;/td&gt; &lt;td style=&quot;width: 80%; border: 1px solid black; border-collapse: collapse; text-align: left;&quot;&gt;" + deleteBILDIRIM.TARIHSAAT + "&lt;/td&gt; &lt;/tr&gt; &lt;tr style=&quot;width: 100%; border: 1px solid black; border-collapse: collapse; background-color: #F7FBFF;&quot;&gt; &lt;td style=&quot;width: 20%; border: 1px solid black; border-collapse: collapse; text-align: left; font-weight: bold;&quot;&gt;Lokasyon&lt;/td&gt; &lt;td style=&quot;width: 80%; border: 1px solid black; border-collapse: collapse; text-align: left;&quot;&gt;" + deleteBILDIRIM.LOKASYON + "&lt;/td&gt; &lt;/tr&gt; &lt;tr style=&quot;width: 100%; border: 1px solid black; border-collapse: collapse; background-color: #FFFFFF;&quot;&gt; &lt;td style=&quot;width: 20%; border: 1px solid black; border-collapse: collapse; text-align: left; font-weight: bold;&quot;&gt;Birim&lt;/td&gt; &lt;td style=&quot;width: 80%; border: 1px solid black; border-collapse: collapse; text-align: left;&quot;&gt;" + deleteBILDIRIM.BIRIM + "&lt;/td&gt; &lt;/tr&gt; &lt;tr style=&quot;width: 100%; border: 1px solid black; border-collapse: collapse; background-color: #F7FBFF;&quot;&gt; &lt;td style=&quot;width: 20%; border: 1px solid black; border-collapse: collapse; text-align: left; font-weight: bold;&quot;&gt;Kategori&lt;/td&gt; &lt;td style=&quot;width: 80%; border: 1px solid black; border-collapse: collapse; text-align: left;&quot;&gt;" + deleteBILDIRIM.KATEGORI + "&lt;/td&gt; &lt;/tr&gt; &lt;tr style=&quot;width: 100%; border: 1px solid black; border-collapse: collapse; background-color: #FFFFFF;&quot;&gt; &lt;td style=&quot;width: 20%; border: 1px solid black; border-collapse: collapse; text-align: left; font-weight: bold;&quot;&gt;Vardiya&lt;/td&gt; &lt;td style=&quot;width: 80%; border: 1px solid black; border-collapse: collapse; text-align: left;&quot;&gt;" + ddlVardiya.Text + "&lt;/td&gt; &lt;/tr&gt; &lt;tr style=&quot;width: 100%; border: 1px solid black; border-collapse: collapse; background-color: #FFFFFF;&quot;&gt; &lt;td style=&quot;width: 20%; border: 1px solid black; border-collapse: collapse; text-align: left; font-weight: bold;&quot;&gt;Bildiren Personel&lt;/td&gt; &lt;td style=&quot;width: 80%; border: 1px solid black; border-collapse: collapse; text-align: left;&quot;&gt;" + deleteBILDIRIM.PERSONEL_AD + "&lt;/td&gt; &lt;/tr&gt; &lt;tr style=&quot;width: 100%; border: 1px solid black; border-collapse: collapse; background-color: #FFFFFF;&quot;&gt; &lt;td style=&quot;width: 20%; border: 1px solid black; border-collapse: collapse; text-align: left; font-weight: bold;&quot;&gt;Bildirim Metni&lt;/td&gt; &lt;td style=&quot;width: 80%; border: 1px solid black; border-collapse: collapse; text-align: left;&quot;&gt;" + deleteBILDIRIM.ACIKLAMA+ "&lt;/td&gt; &lt;/tr&gt; &lt;/tbody&gt; &lt;/table&gt; &lt;/td&gt; &lt;/tr&gt; &lt;tr&gt; &lt;td style=&quot;background-color: #4791d2; border-top: 2px solid #367fbe; height: 6px;&quot;&gt;&lt;/td&gt; &lt;/tr&gt; &lt;/tbody&gt; &lt;/table&gt; &lt;/td&gt; &lt;/tr&gt; &lt;/tbody&gt; &lt;/table&gt;";

                    if (ms.MailGonder(mailOnay.SILME_KAYIT, MudahilMail, "Bildirim Silimi", HttpUtility.HtmlDecode(c).ToString()))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "myFunction('Bildirim silindi! Bilgilendirme Maili Gönderildi','succsess');", true);
                    }



                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "myFunction('Bildirim silindi','succsess');", true);


                    bind();

                    


                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "Myfunction('iş Tanımı silme işlemi sırasında Hata oluştu','fail');", true);
                    grdBILDIRIM.DataBind();
                }
            }
            catch (Exception)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "myFunction('Hay Aksi!  Beklenmedik bir hata oluştu :(','fail');", true);
            }


           




        }

        protected void btn_edit_link_Click(object sender, EventArgs e)
        {
            try
            {
                BILDIRIMLVL1 edit = new BILDIRIMLVL1();

                LinkButton linkbutton = (LinkButton)sender;  // get the link button which trigger the event
                GridViewRow row = (GridViewRow)linkbutton.NamingContainer; // get the GridViewRow that contains the linkbutton

                int editID = Convert.ToInt32(linkbutton.CommandArgument);
                edit = db.BILDIRIMLVL1.FirstOrDefault(i => i.ID_BILDIRIM == editID);
                var FullDate = edit.TARIHSAAT;
                string[] parce = FullDate.Split(' ');



                IDsecret.Text = Convert.ToString(edit.ID_BILDIRIM);
                txtTarih_Edit.Text = parce[0];
                txtSaat_Edit.Text = parce[1];
                ddlMudahilPersonel_edit.Text = edit.MUDAHIL_PERSONEL.ToUpper();
                ddlKategori_Edit.Text = edit.KATEGORI;
                txtBildirimMetin_Edit.Text = edit.ACIKLAMA;
                ddlOnlemBool_edit.Text = edit.ONLEMBOOL;
                ddlBirim_Edit.Text = edit.BIRIM;
                ddl_Lokasyon_Edit.Text = edit.LOKASYON;
                ddlBildirimYapan_Edit.Text = edit.PERSONEL_AD.ToUpper();
                ass.ImageUrl = "~/Upload/" + edit.GORSEL.ToString();
                ddlDurumEdit.Text = edit.DURUM;
                ScriptManager.RegisterStartupScript(this, GetType(), "serkan", "$('#myModal').modal()", true);//show the modal
            }
            catch (Exception)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "myFunction('Hay Aksi!  Beklenmedik bir hata oluştu :(','fail');", true);
            }
           



        }

        protected void ddlAktiflik_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlAktiflik.SelectedValue == "Pasif")
            {
                grdBILDIRIM.DataSource = db.BILDIRIMLVL1.Where(i => i.DURUM == "Pasif").ToList();
                grdBILDIRIM.DataBind();

            }
            else
            {

                grdBILDIRIM.DataSource = db.BILDIRIMLVL1.Where(i => i.DURUM == "Aktif").ToList();
                grdBILDIRIM.DataBind();



            }
        }

        protected void edit_Kaydet_Click(object sender, EventArgs e)
        {
            try
            {
                //
                BILDIRIMLVL1 edit = new BILDIRIMLVL1();

                int editID = Convert.ToInt32(IDsecret.Text);

                edit = db.BILDIRIMLVL1.FirstOrDefault(i => i.ID_BILDIRIM == editID);

                string tarih = txtTarih_Edit.Text;
                string saat = txtSaat_Edit.Text;
                string Date = tarih + " " + saat;
                edit.TARIHSAAT = Date;
                edit.VARDIYA = ddlVardiya_Edit.SelectedValue;
                edit.PERSONEL_AD = ddlBildirimYapan_Edit.SelectedValue;
                edit.BIRIM = ddlBirim_Edit.SelectedValue;
                edit.KATEGORI = ddlKategori_Edit.SelectedValue;
                edit.LOKASYON = ddl_Lokasyon_Edit.SelectedValue;
                edit.ONLEMBOOL = ddlOnlemBool_edit.SelectedValue;
                edit.DURUM = ddlDurumEdit.SelectedValue;
                edit.ACIKLAMA = txtBildirimMetin_Edit.Text;
                edit.AKSIYON = txtAksiyon_Edit.Text;
               if(edit.GORSEL.ToUpper() == imgEdit.FileName.ToString())
                {
                    edit.GORSEL = imgEdit.FileName.ToString();
                }
                edit.MUDAHIL_PERSONEL = ddlMudahilPersonel_edit.SelectedValue;

                if (db.SaveChanges() > 0)
                {


                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "myFunction('Bildirim Güncellendi','succsess');", true);
                        bind();

                }
                else
                {

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "myFunction('Bildirim güncellenemedi','fail');", true);

                }



            }
            catch (Exception)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "myFunction('Hay Aksi!  Beklenmedik bir hata oluştu :(','fail');", true);
            }
           







        }

        protected void reload_Click(object sender, EventArgs e)
        {


            try
            {
                LinkButton linkbutton = (LinkButton)sender;  // get the link button which trigger the event
                GridViewRow row = (GridViewRow)linkbutton.NamingContainer; // get the GridViewRow that contains the linkbutton
                BILDIRIMLVL1 aktif = new BILDIRIMLVL1();

                int aktifID = Convert.ToInt32(linkbutton.CommandArgument);
                aktif = db.BILDIRIMLVL1.FirstOrDefault(i => i.ID_BILDIRIM == aktifID);
                aktif.DURUM = "Aktif";
                aktif.BILDIRIM_DURUM = "Onay Bekliyor";
                if (db.SaveChanges() > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "myFunction('Kayıt başarıyla aktife alındı','succsess');", true);
                    bind();
                }
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "Myfunction('Aktife alma işlemi sırasında Hata oluştu','fail');", true);
                    bind();
                }

            }
            catch (Exception)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "myFunction('Hay Aksi!  Beklenmedik bir hata oluştu :(','fail');", true);
            }






        }
    }
}