using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Numerics;
using FFMpegCore;
using FFMpegCore.Pipes;
using FFMpegCore.Extensions.System.Drawing.Common;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Net.Http.Json;
using System.Runtime.Serialization;
#pragma warning disable CA1416
class Program
{
    public static Pen[] pens = {new Pen(Color.Red), new Pen(Color.OrangeRed), new Pen(Color.DarkOrange), new Pen(Color.Orange), new Pen(Color.Gold), new Pen(Color.Yellow), 
                                new Pen(Color.GreenYellow), new Pen(Color.Lime), new Pen(Color.LimeGreen), new Pen(Color.ForestGreen), new Pen(Color.SeaGreen), new Pen(Color.Turquoise),
                                new Pen(Color.Cyan), new Pen(Color.DodgerBlue), new Pen(Color.RoyalBlue), new Pen(Color.MediumSlateBlue), new Pen(Color.BlueViolet), new Pen(Color.DarkMagenta),
                                new Pen(Color.MediumVioletRed), new Pen(Color.Crimson)};

    public static long c = 0;
    public static int width = 2000, height = 2000;
    public struct vec2{
        public vec2(double X, double Y){
            x = X;
            y = Y;
        }
        public double x;
        public double y;

        public void rotate(double a){
            a = Math.PI / 180 * a;
            double x1 = x, y1 = y;
            x = Math.Cos(a)*x1 - Math.Sin(a)*y1;
            y = Math.Sin(a)*x1 + Math.Cos(a)*y1;
        }

        public static vec2 operator -(vec2 a, vec2 b){
            return new vec2(a.x-b.x, a.y-b.y);
        }
        public static vec2 operator +(vec2 a, vec2 b){
            return new vec2(a.x+b.x, a.y+b.y);
        }
        public static vec2 operator /(vec2 a, double b){
            return new vec2(a.x/b, a.y/b);
        }
        public static vec2 operator *(vec2 a, double b){
            return new vec2(a.x*b, a.y*b);
        }

        public static vec2 operator *(vec2 a, vec2 b){
            return new vec2(a.x*b.x, a.y*b.y);
        }
        public double dist(){
            return Math.Sqrt(x*x+y*y);
        }


    }

    public static Bitmap sierpinski_triangle(int d, Bitmap image, vec2 startp, vec2 endp, int dir){    //startp: absolute position, endp: relativ zu startp
        // Pen pen = pens[d%20];
        // pen.Width = d;
        // Graphics graphics = Graphics.FromImage(image);
        // vec2 temp = endp + startp;
        // graphics.DrawLine(pen, new PointF((float)startp.x, (float)startp.y), new PointF((float)temp.x, (float)temp.y));
        if(d > 0){
            endp/=2;
            endp.rotate(60.0*dir);
            sierpinski_triangle(d-1, image, startp, endp, dir*-1);
            
            startp += endp;
            endp.rotate(-60.0 * dir);
            sierpinski_triangle(d-1, image, startp, endp, dir);
            startp += endp;
            endp.rotate(-60.0 * dir);
            sierpinski_triangle(d-1, image, startp, endp, dir*-1);
        }
        else{
            c += 1;
            Pen pen = pens[c%20];
            pen.Width = 5;
            Graphics graphics = Graphics.FromImage(image);
            endp += startp;
            graphics.DrawLine(pen, new PointF((float)startp.x, (float)startp.y), new PointF((float)endp.x, (float)endp.y));
        }
        
        return image;
    }

    public static Bitmap dragon_curve(int d, Bitmap image, vec2 startp, vec2 endp, int dir){
        //         Pen pen = pens[d%20];
        // pen.Width = d;
        // Graphics graphics = Graphics.FromImage(image);
        // vec2 temp = endp + startp;
        // graphics.DrawLine(pen, new PointF((float)startp.x, (float)startp.y), new PointF((float)temp.x, (float)temp.y));
        // Console.WriteLine("d: " + d +" dir: " + dir + " "+ startp.x + " " + startp.y + " " + endp.x + " " + endp.y);
        if(d>0){
            double dist = endp.dist();
            endp /= endp.dist() / (dist / Math.Sqrt(2));
            endp.rotate(45*dir);
            dragon_curve(d-1, image, startp, endp, 1);
            startp += endp;
            endp.rotate(-90*dir);
            dragon_curve(d-1, image, startp, endp, -1);
        }
        else{
            c+=1;
            Pen pen = pens[c%20];
            pen.Width = 1;
            Graphics graphics = Graphics.FromImage(image);
            endp += startp;
            graphics.DrawLine(pen, new PointF((float)startp.x, (float)startp.y), new PointF((float)endp.x, (float)endp.y));
        }

        
        return image;
    }

    public static Bitmap levy_c(int d, Bitmap image, vec2 startp, vec2 endp, int dir){
        //         Pen pen = pens[d%20];
        // pen.Width = d;
        // Graphics graphics = Graphics.FromImage(image);
        // vec2 temp = endp + startp;
        // graphics.DrawLine(pen, new PointF((float)startp.x, (float)startp.y), new PointF((float)temp.x, (float)temp.y));
        // Console.WriteLine("d: " + d +" dir: " + dir + " "+ startp.x + " " + startp.y + " " + endp.x + " " + endp.y);
        if(d>0){
            double dist = endp.dist();
            endp /= endp.dist() / (dist / Math.Sqrt(2));
            endp.rotate(45*dir);
            levy_c(d-1, image, startp, endp, dir);
            startp += endp;
            endp.rotate(-90*dir);
            levy_c(d-1, image, startp, endp, dir);
        }
        else{
            c+=1;
            Pen pen = pens[c%20];
            pen.Width = 1;
            Graphics graphics = Graphics.FromImage(image);
            endp += startp;
            graphics.DrawLine(pen, new PointF((float)startp.x, (float)startp.y), new PointF((float)endp.x, (float)endp.y));
        }
        return image;
    }

    public static Bitmap golden_dragon_curve_e(int d, Bitmap image, vec2 startp, vec2 endp, int dir){
        Pen pen = pens[d%20];
        pen.Width = 1;
        Graphics graphics = Graphics.FromImage(image);
        vec2 temp = endp + startp;
        graphics.DrawLine(pen, new PointF((float)startp.x, (float)startp.y), new PointF((float)temp.x, (float)temp.y));
        if(dir == 1){
            vec2 temp2 = startp + endp;
            endp = startp - temp2;
            startp = temp2;
        }
        if(d>0){

            endp *= 0.74274;
            endp.rotate(-32.893818);
            golden_dragon_curve_e(d-1, image, startp, endp, 1);
            startp += endp;
            endp *= 0.74274;
            endp.rotate(79.87990025);
            golden_dragon_curve_e(d - 1, image, startp, endp, 0);
        }
        // else{
        //     c+=1;
        //     Pen pen = pens[c%20];
        //     pen.Width = 5;
        //     Graphics graphics = Graphics.FromImage(image);
        //     endp += startp;
        //     graphics.DrawLine(pen, new PointF((float)startp.x, (float)startp.y), new PointF((float)endp.x, (float)endp.y));
        // }

        return image;
    }
    public static Bitmap golden_dragon_curve(int d, Bitmap image, vec2 startp, vec2 endp, int dir){
        int cmax = 10000;
        if(c>cmax){
            return image;
        }
        Pen pen = pens[d%20];
        pen.Width = 1;
        if(d>10){
            pen.Width = 5;
        }
        c += 1;
        Graphics graphics = Graphics.FromImage(image);
        vec2 temp = endp + startp;
        graphics.DrawLine(pen, new PointF((float)startp.x, (float)startp.y), new PointF((float)temp.x, (float)temp.y));


        if(d>0){
            if(dir == -1){
                endp *= 0.74274*0.74274;
                endp.rotate(46.98598225);
                golden_dragon_curve(d-1, image, startp, endp, 1);
                startp += endp;
                endp /= 0.74274;
                endp.rotate(-79.87990025);
                golden_dragon_curve(d - 1, image, startp, endp, -1);
            }

            else{
                endp *= 0.74274;
                endp.rotate(-32.893818);
                golden_dragon_curve(d-1, image, startp, endp, 1);
                startp += endp;
                endp *= 0.74274;
                endp.rotate(79.87990025);
                golden_dragon_curve(d - 1, image, startp, endp, -1);
            }
        }
        // else{
        //     c+=1;
        //     Pen pen = pens[c%20];
        //     pen.Width = 1;
        //     Graphics graphics = Graphics.FromImage(image);
        //     endp += startp;
        //     graphics.DrawLine(pen, new PointF((float)startp.x, (float)startp.y), new PointF((float)endp.x, (float)endp.y));
        // }

        return image;
    }

    public static Bitmap iterative_golden_dragon_curve(int d, Bitmap image, vec2 startpos, vec2 endpos){
        Queue<Tuple<vec2, vec2>> elements = new Queue<Tuple<vec2, vec2>>();

        elements.Enqueue(new Tuple<vec2,vec2>(startpos, endpos));
        Graphics graphics = Graphics.FromImage(image);
        int maxc = 10000; 
        for(int i = 0; i<d; i++){
            int s = elements.Count();
            if(c>maxc){break;}
            Pen pen = pens[i%20];
            // pen.Width = 5;
            for(int j = 0; j<s; j++){
                c += 1;
                if(c>maxc){break;}
                vec2 startp = elements.First().Item1;
                vec2 endp = elements.First().Item2;
                elements.Dequeue();
                if(i<10){
                    pen.Width = 5;
                }
                vec2 temp = endp + startp;
                graphics.DrawLine(pen, new PointF((float)startp.x, (float)startp.y), new PointF((float)temp.x, (float)temp.y));
            
                if(j % 2 != 0){
                    endp *= 0.74274*0.74274;
                    endp.rotate(46.98598225);
                    elements.Enqueue(new Tuple<vec2, vec2>(startp, endp));
                    startp += endp;
                    endp /= 0.74274;
                    endp.rotate(-79.87990025);
                    elements.Enqueue(new Tuple<vec2, vec2>(startp, endp));
                }
                else{
                    endp *= 0.74274;
                    endp.rotate(-32.893818);
                    elements.Enqueue(new Tuple<vec2, vec2>(startp, endp));
                    startp += endp;
                    endp *= 0.74274;
                    endp.rotate(79.87990025);
                    elements.Enqueue(new Tuple<vec2, vec2>(startp, endp));
                }
            }
        }
        
        Pen pen2 = pens[d%20];
        pen2.Width = 1;
        while(elements.Any()){
            c += 1;
            if(c>maxc){break;}
            vec2 startp = elements.Peek().Item1;
            vec2 endp = elements.First().Item2;
            elements.Dequeue();
            vec2 temp = endp + startp;
            graphics.DrawLine(pen2, new PointF((float)startp.x, (float)startp.y), new PointF((float)temp.x, (float)temp.y));
        }
        return image;
    }

    public static IEnumerable<BitmapVideoFrameWrapper> frames_sierpinski_iterative(int d, Bitmap image, vec2 startp, vec2 endp){
        Queue<Tuple<Tuple<vec2, vec2>, int>> elements = new Queue<Tuple<Tuple<vec2, vec2>, int>>();
        elements.Enqueue(new Tuple<Tuple<vec2, vec2>, int>(new Tuple<vec2, vec2>(startp, endp),-1));
        Graphics graphics = Graphics.FromImage(image);
        for(int i = 0; i<d; i++){
            Pen pen = pens[i*2%20];
            pen.Width = 5;
            int s = elements.Count();

            for(int j = 0; j<s; j++){
            
                startp = elements.First().Item1.Item1;
                endp = elements.First().Item1.Item2;
                int dir = elements.First().Item2;
                elements.Dequeue();

                if((j+2)< s && j%3 == 0){
                    vec2 tempend = endp *2;
                    tempend.rotate(60.0*dir);
                    vec2 tempp = tempend+startp;
                    graphics.DrawLine(new Pen(Color.Black, 5), new PointF((float)startp.x, (float)startp.y), new PointF((float)tempp.x, (float)tempp.y));
                }

                vec2 temp = endp + startp;
                graphics.DrawLine(pen, new PointF((float)startp.x, (float)startp.y), new PointF((float)temp.x, (float)temp.y));
                BitmapVideoFrameWrapper rimage = new(image);
                yield return rimage;

                endp /= 2;
                endp.rotate(60.0*dir);
                elements.Enqueue(new Tuple<Tuple<vec2, vec2>, int>( new Tuple<vec2, vec2>(startp, endp),dir*-1));
                startp += endp;
                endp.rotate(-60.0 * dir);
                elements.Enqueue(new Tuple<Tuple<vec2, vec2>, int>( new Tuple<vec2, vec2>(startp, endp),dir));
                startp += endp;
                endp.rotate(-60.0 * dir);
                elements.Enqueue(new Tuple<Tuple<vec2, vec2>, int>( new Tuple<vec2, vec2>(startp, endp),dir*-1));
            
            }


        }


    }


    public static IEnumerable<BitmapVideoFrameWrapper> frames_sierpinski(int d, Bitmap image, vec2 startp, vec2 endp, int dir)
    {
        vec2 oendp = endp;
        vec2 ostartp = startp;
        Pen pen = pens[19-((d*2)%20)];
        pen.Width = 5;
        Graphics graphics = Graphics.FromImage(image);
        vec2 temp = endp + startp;
        graphics.DrawLine(pen, new PointF((float)startp.x, (float)startp.y), new PointF((float)temp.x, (float)temp.y));
        if(d > 0){
            endp/=2;
            endp.rotate(60.0*dir);
            foreach(BitmapVideoFrameWrapper frame in frames_sierpinski(d-1, image, startp, endp, dir*-1)){
                yield return frame;
            }
            
            startp += endp;
            endp.rotate(-60.0 * dir);
            foreach(BitmapVideoFrameWrapper frame in frames_sierpinski(d-1, image, startp, endp, dir)){
                yield return frame;
            }
            startp += endp;
            endp.rotate(-60.0 * dir);
            foreach(BitmapVideoFrameWrapper frame in frames_sierpinski(d-1, image, startp, endp, dir*-1)){
                yield return frame;
            }
            Pen erase = new Pen(Color.Black);
            erase.Width = 5;
            endp = oendp;
            startp = ostartp;
            graphics.DrawLine(erase, new PointF((float)startp.x, (float)startp.y), new PointF((float)temp.x, (float)temp.y));
            endp/=2;
            endp.rotate(60.0*dir);
            var f1 = frames_sierpinski(d-1, image, startp, endp, dir*-1);
            int c1 = f1.Count();
            int cc1 = 0;
            foreach(BitmapVideoFrameWrapper frame in f1){
                cc1++;
                if(cc1 == c1){
                    yield return frame;
                }
            }

            startp += endp;
            endp.rotate(-60.0 * dir);
            f1 = frames_sierpinski(d-1, image, startp, endp, dir);
            c1 = f1.Count();
            cc1 = 0;
            foreach(BitmapVideoFrameWrapper frame in f1){
                cc1++;
                if(cc1 == c1){
                    yield return frame;
                }
            }

            startp += endp;
            endp.rotate(-60.0 * dir);
            f1 = frames_sierpinski(d-1, image, startp, endp, dir*-1);
            
            c1 = f1.Count();
            cc1 = 0;
            foreach(BitmapVideoFrameWrapper frame in f1){
                cc1++;
                if(cc1 == c1){
                    yield return frame;
                }
            }

        }
        
        // else{
        //     c += 1;
        //     Pen pen = pens[c%20];
        //     pen.Width = 5;
        //     Graphics graphics = Graphics.FromImage(image);
        //     endp += startp;
        //     graphics.DrawLine(pen, new PointF((float)startp.x, (float)startp.y), new PointF((float)endp.x, (float)endp.y));
        // }
        BitmapVideoFrameWrapper rimage = new(image);
        yield return rimage;
    }
    public static IEnumerable<BitmapVideoFrameWrapper> CreateFramesSD(int count, int width, int height)
    {
        for (int i = 0; i < count; i++)
        {
            // create a Bitmap
            using Bitmap bmp = new(width, height);
            using Graphics gfx = Graphics.FromImage(bmp);

            // draw a blue background
            gfx.Clear(Color.Navy);

            // draw a growing green square
            Point pt = new(i, i);
            Size sz = new(i, i);
            Rectangle rect = new(pt, sz);
            gfx.FillRectangle(Brushes.Green, rect);

            // draw some text
            using Font fnt = new("consolas", 24);
            gfx.DrawString($"Frame: {i + 1:N0}", fnt, Brushes.Yellow, 2, 2);

            // yield the wrapped Bitmap
            using BitmapVideoFrameWrapper wrappedBitmap = new(bmp);
            yield return wrappedBitmap;
        }
    }

    public static Color color_mandelbrot(int d, double zx, double zy, double cx, double cy){
        if(d > 2000){
            return Color.Black;
        }
        if(zx*zx + zy*zy >= 2*2){
            return pens[(d/100)%20].Color;
        }
        return color_mandelbrot(d+1, zx*zx-zy*zy+cx, 2*zx*zy+cy, cx, cy);
    }

    public static Bitmap mandelbrotset(Bitmap image){
        float cx = 0, cy = 0;
        for(int i = 0; i<width; i++){
            // cx =  ((i/width) * 2.48) - 2;
            cx = (float)i/width;
            cx *= (float)2.48/8192;   ///1
            cx -= (float)0.9884;      //2
            for(int j = 0; j<(int)height*(float)(2.25/2.48); j++){
                cy = (float)j/height;
                cy *= (float)2.25/8192;    ///1
                cy -= (float)0.2511;       //1.12
                image.SetPixel(i,j, color_mandelbrot(0,0,0,cx,cy));
            }
        }
        
        
        return image;
    }


    public static Color color_julia(int d, double zx, double zy, double cx, double cy){
        if(d > 500){
            return Color.Black;
        }
        if(zx*zx + zy*zy >= 2*2){
            return pens[19-((d/2)%20)].Color;
        }
        return color_julia(d+1, zx*zx-zy*zy+cx, 2*zx*zy+cy, cx, cy);
    }

    public static Bitmap juliaset(Bitmap image, float cx1 = 0, float cy1 = 0){
        float cx = 0, cy = 0;
        for(int i = 0; i<width; i++){
            // cx =  ((i/width) * 2.48) - 2;
            cx = (float)i/width;
            cx *= (float)3.8/1;   ///1
            cx -= (float)1.9;      //2
            for(int j = 0; j<(int)height*(float)(10/10); j++){
                cy = (float)j/height;
                cy *= (float)3.8/1;    ///1
                cy -= (float)1.9;       //1.12
                image.SetPixel(i,j, color_julia(0,cx,cy,cx1,cy1));
            }
        }


        return image;
    }
    static void Main()
    {
        // Create a blank image with a specified width and height
        // int width = 16001;
        // int height = 16001;
        Bitmap image = new Bitmap(width, height);

        // Create a graphics object from the image
        Graphics graphics = Graphics.FromImage(image);
        graphics.Clear(Color.Black);
        // Draw a red rectangle on the image
        // image = mandelbrotset(image);

        // for(float i = -1; i<1; i +=  (float)0.1){
        //     for(float j = (float)-1.4; j<0.6; j += (float)0.1){
        //         image = new Bitmap(width, height);
        //         image = juliaset(image,i,j);
        //         string path = "../../../image"+Convert.ToString(i)+"_"+Convert.ToString(j)+".jpg";
        //         image.Save(path);
        //     }
        // }

        // image = juliaset(image, (float)-0.8, (float)-0.2);

        // image = sierpinski_triangle(5, image, new vec2(0, 16000.0), new vec2(16000,0), -1);
        // image = dragon_curve(24, image, new vec2(3500,10000), new vec2(9000,-6500), 1);
        // image = levy_c(25,image, new vec2(3000,3000), new vec2(2000,0), 1);
        // image = levy_c(15,image, new vec2(2000,7000), new vec2(4000,0), -1);
        // image = golden_dragon_curve(19, image, new vec2(1500, 2000), new vec2(5000, 5000), 1);
        // image = iterative_golden_dragon_curve(19, image, new vec2(1500, 2000), new vec2(5000, 5000));
        // Pen pen = new Pen(Color.Black);
        // Rectangle rectangle = new Rectangle(50, 50, 200, 100);
        // graphics.DrawRectangle(pen, rectangle);
        Console.WriteLine(c);
        // Save the image to a file
        // string filePath = "../../../image.jpg";
        // image.Save(filePath);

        // var frames = CreateFramesSD(count: 200, width: 400, height: 300);
        var frames = frames_sierpinski(6, image, new vec2(0,width-1), new vec2(height-1,0),-1);
        // var frames = frames_sierpinski_iterative(7, image, new vec2(0, width-1), new vec2(height-1,0));
        RawVideoPipeSource source = new(frames) { FrameRate = 40};
        bool success = FFMpegArguments.FromPipeInput(source).OutputToFile("../../../outputee.webm", overwrite: true, options => options.WithVideoCodec("libvpx-vp9")).ProcessSynchronously();


        // Dispose of the graphics object and image
        graphics.Dispose();
        image.Dispose();

        Console.WriteLine("Image generated and saved successfully.");
    }
}